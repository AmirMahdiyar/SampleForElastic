using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SampleForElastic.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Infraustructure.Repositories
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;
        private readonly ILogger<UserReadRepository> _logger;

        public UserReadRepository(ElasticsearchClient client, IConfiguration configuration, ILogger<UserReadRepository> logger)
        {
            _client = client;
            _logger = logger;
            // Best Practice: Load index name from configuration with a reliable fallback
            _indexName = configuration["Elasticsearch:IndexName"] ?? "users-read-index";
        }

        public async Task InitializeIndexAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Checking if Elasticsearch index '{IndexName}' exists...", _indexName);
                
                var existsResponse = await _client.Indices.ExistsAsync(_indexName, ct);
                
                if (existsResponse.IsValidResponse && existsResponse.Exists)
                {
                    _logger.LogInformation("Elasticsearch index '{IndexName}' already exists.", _indexName);
                    return;
                }

                _logger.LogInformation("Elasticsearch index '{IndexName}' does not exist. Creating with custom mapping configuration...", _indexName);

                var createIndexResponse = await _client.Indices.CreateAsync(_indexName, c => c
                    // CHOICE: Optimize shards and replicas for local / single-node operations.
                    // WHY: Multiple shards on a single node add overhead. 1 shard is sufficient. 0 replicas prevent "Yellow" status on single-node setups.
                    .Settings(s => s
                        .NumberOfShards(1)
                        .NumberOfReplicas(0)
                    )
                    // CHOICE: Explicit mappings definitions to prevent Elastic dynamic mapping from inferring sub-optimal types.
                    // WHY: Forces exact matching on IDs/Codes, enables proper full-text search on description/names, and optimizes memory usage.
                    .Mappings(m => m
                        .Properties<UserSearchModel>(p => p
                            // Target Guid Id mapped as Keyword.
                            // WHY: Guid IDs are exact values, never tokenized, and searched using Term queries. Keyword type is optimal for filter performance.
                            .Keyword(x => x.Id)
                            
                            // Username mapped as Keyword.
                            // WHY: Usernames are exact identifier strings. We want exact match check and case-sensitive sorting.
                            .Keyword(x => x.Username)
                            
                            // BirthDate mapped as Date.
                            // WHY: Dates need to be queried using range filters (e.g. users older than X).
                            .Date(x => x.BirthDate)
                            
                            // About mapped as Text.
                            // WHY: 'About' is a bio/description meant for full-text search. It must be tokenized by the standard analyzer to support keyword searching.
                            .Text(x => x.About)
                            
                            // CreatedAt mapped as Date.
                            // WHY: Essential for ordering search results chronologically.
                            .Date(x => x.CreatedAt)
                            
                            // Cars collection mapped as standard Object (flat representation).
                            // WHY: By using a standard Object instead of nested mapping, Elastic flattens the properties.
                            // Full-text searches on car properties work seamlessly alongside profile searches without needing complex nested query syntax.
                            .Object(x => x.Cars, o => o
                                .Properties(cp => cp
                                    // Car ID.
                                    // WHY: Exact lookup match, no tokenization.
                                    .Keyword("id")
                                    
                                    // Car Name/Model.
                                    // WHY: Mapped as Text with a Keyword subfield. This supports both full-text search (e.g., brand lookup) and exact sorting/aggs.
                                    .Text("name", t => t.Fields(f => f.Keyword("keyword")))
                                    
                                    // Car Code/Plate.
                                    // WHY: License plates/codes are exact strings, never tokenized.
                                    .Keyword("code")
                                    
                                    // Car Color.
                                    // WHY: Colors are exact matches (e.g., 'Red', 'Blue'). Mapped as Keyword.
                                    .Keyword("color")
                                )
                            )
                        )
                    ), ct);

                if (!createIndexResponse.IsValidResponse)
                {
                    throw new Exception($"Failed to create index in Elasticsearch: {createIndexResponse.DebugInformation}");
                }

                _logger.LogInformation("Elasticsearch index '{IndexName}' created successfully.", _indexName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Elasticsearch index '{IndexName}' initialization.", _indexName);
                throw;
            }
        }

        public async Task<UserSearchModel?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var response = await _client.GetAsync<UserSearchModel>(id.ToString(), g => g.Index(_indexName), ct);
            
            if (response.IsValidResponse && response.Found)
            {
                return response.Source;
            }

            return null;
        }

        public async Task<IEnumerable<UserSearchModel>> SearchAsync(string searchTerm, CancellationToken ct)
        {
            // CHOICE: MultiMatch Query across relevant searchable fields.
            // WHY: Provides clean, full-text search capabilities across username, bio (about), and denormalized car names/colors.
            var response = await _client.SearchAsync<UserSearchModel>(s => s
                .Index(_indexName)
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(new[] { "username", "about", "cars.name", "cars.color" })
                        .Query(searchTerm)
                        .Fuzziness(Fuzziness.Auto) // Allow minor spelling mistakes (fuzzy search)
                    )
                ), ct);

            if (response.IsValidResponse)
            {
                return response.Documents;
            }

            _logger.LogError("Search query failed: {DebugInfo}", response.DebugInformation);
            return Array.Empty<UserSearchModel>();
        }

        public async Task IndexAsync(UserSearchModel model, CancellationToken ct)
        {
            // CHOICE: Idempotent indexing by setting document ID explicitly to the Aggregate ID.
            // WHY: If the outbox sync processes the same event multiple times (due to retries or network faults),
            // it will overwrite/update the existing document rather than creating a duplicate.
            var response = await _client.IndexAsync(model, idx => idx
                .Index(_indexName)
                .Id(model.Id.ToString()), ct);

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to index document in Elasticsearch: {response.DebugInformation}");
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var response = await _client.DeleteAsync<UserSearchModel>(id.ToString(), d => d.Index(_indexName), ct);

            // Note: If document does not exist, it is already considered deleted (idempotent delete success).
            if (!response.IsValidResponse && response.ElasticsearchServerError?.Status != 404)
            {
                throw new Exception($"Failed to delete document from Elasticsearch: {response.DebugInformation}");
            }
        }
    }
}
