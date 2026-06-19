using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using SampleForElastic.Application.Contracts;

namespace Mokeb.Infrastructure.Repositories
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        public UserReadRepository(ElasticsearchClient client, IConfiguration configuration)
        {
            _client = client;
            _indexName = configuration["Elasticsearch:IndexName"] ?? "users-read-index";
        }

        public async Task<bool> InitializeIndexAsync(CancellationToken ct)
        {
            try
            {
                var existsResponse = await _client.Indices.ExistsAsync(_indexName, ct);


                if (!existsResponse.Exists)
                {
                    var createIndexResponse = await _client.Indices.CreateAsync(_indexName, c => c
                        .Settings(s => s
                            .NumberOfShards(1)
                            .NumberOfReplicas(0)
                        )
                        .Mappings(m => m
                            .Properties<UserSearchModel>(p => p
                                .Keyword(x => x.Id)
                                .Keyword(x => x.Username)
                                .Date(x => x.BirthDate)
                                .Text(x => x.About)
                                .Date(x => x.CreatedAt)
                                .Object(x => x.Cars, o => o
                                    .Properties(cp => cp
                                        .Keyword("id")
                                        .Text("name", t => t.Fields(f => f.Keyword("keyword")))
                                        .Keyword("code")
                                        .Keyword("color")
                                    )
                                )
                            )
                        ), ct);

                    if (!createIndexResponse.IsValidResponse)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserSearchModel?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var response = await _client.GetAsync<UserSearchModel>(id.ToString(), g => g.Index(_indexName), ct);
                if (response.IsValidResponse && response.Found)
                {
                    return response.Source;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserSearchModel>> SearchAsync(string searchTerm, CancellationToken ct)
        {
            try
            {
                var response = await _client.SearchAsync<UserSearchModel>(s => s
                    .Index(_indexName)
                    .Query(q => q
                        .MultiMatch(m => m
                            .Fields(new[] { "username", "about", "cars.name", "cars.color" })
                            .Query(searchTerm)
                            .Fuzziness(new Fuzziness("auto"))
                        )
                    ), ct);

                if (!response.IsValidResponse)
                {
                    return Array.Empty<UserSearchModel>();
                }

                return response.Documents;
            }
            catch
            {
                return Array.Empty<UserSearchModel>();
            }
        }

        public async Task<bool> IndexAsync(UserSearchModel model, CancellationToken ct)
        {
            try
            {
                var response = await _client.IndexAsync(model, idx => idx
                    .Index(_indexName)
                    .Id(model.Id.ToString()), ct);

                return response.IsValidResponse;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var response = await _client.DeleteAsync<UserSearchModel>(id.ToString(), d => d.Index(_indexName), ct);
                if (!response.IsValidResponse && response.ElasticsearchServerError?.Status != 404)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddCarAsync(Guid userId, CarSearchModel car, CancellationToken ct)
        {
            try
            {
                var existingUserResponse = await _client.GetAsync<UserSearchModel>(userId.ToString(), g => g.Index(_indexName), ct);
                if (!existingUserResponse.IsValidResponse || !existingUserResponse.Found)
                {
                    return false;
                }

                var existingUser = existingUserResponse.Source;
                if (existingUser != null)
                {
                    existingUser.Cars ??= new List<CarSearchModel>();
                    existingUser.Cars.Add(car);

                    var response = await _client.UpdateAsync<UserSearchModel, object>(userId.ToString(), u => u
                        .Index(_indexName)
                        .Doc(new { Cars = existingUser.Cars }), ct);

                    return response.IsValidResponse;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveCarAsync(Guid userId, Guid carId, CancellationToken ct)
        {
            try
            {
                var existingUserResponse = await _client.GetAsync<UserSearchModel>(userId.ToString(), g => g.Index(_indexName), ct);
                if (!existingUserResponse.IsValidResponse || !existingUserResponse.Found)
                {
                    return false;
                }

                var existingUser = existingUserResponse.Source;
                if (existingUser != null && existingUser.Cars != null)
                {
                    existingUser.Cars = existingUser.Cars.Where(c => c.Id != carId).ToList();

                    var response = await _client.UpdateAsync<UserSearchModel, object>(userId.ToString(), u => u
                        .Index(_indexName)
                        .Doc(new { Cars = existingUser.Cars }), ct);

                    return response.IsValidResponse;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserFieldsAsync(Guid userId, string username, string about, DateOnly birthDate, CancellationToken ct)
        {
            try
            {
                var response = await _client.UpdateAsync<UserSearchModel, object>(userId.ToString(), u => u
                    .Index(_indexName)
                    .Doc(new { Username = username, About = about, BirthDate = birthDate }), ct);

                return response.IsValidResponse;
            }
            catch
            {
                return false;
            }
        }
    }
}
