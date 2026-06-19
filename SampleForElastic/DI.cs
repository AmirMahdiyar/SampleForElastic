using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;
using Mokeb.Infrastructure.Repositories;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Commands.Handler.CreateUser;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Infraustructure;
using SampleForElastic.Infraustructure.Interceptors;
using SampleForElastic.Infraustructure.Repositories;

namespace SampleForElastic
{
    public static class DI
    {
        public static IServiceCollection AddDI(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<SampleDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                opt.AddInterceptors(new EventsInterceptor());
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IOutboxRepository, OutboxRepository>();

            var elasticUrl = configuration["Elasticsearch:Url"] ?? "https://localhost:9200";
            var defaultIndex = configuration["Elasticsearch:IndexName"] ?? "users-read-index";

            var settings = new ElasticsearchClientSettings(new Uri(elasticUrl))
                .DefaultIndex(defaultIndex)
                .EnableDebugMode()
                .ServerCertificateValidationCallback((sender, certificate, chain, errors) => true)
                .Authentication(new BasicAuthentication("elastic", "yXKRTf=WOXmBxgyNRjzo"));

            services.AddSingleton(new ElasticsearchClient(settings));
            services.AddScoped<IUserReadRepository, UserReadRepository>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(CreateUserCommand).Assembly
                );
            });

            return services;
        }
    }
}

