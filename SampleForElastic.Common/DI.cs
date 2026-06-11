using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleForElastic.Infraustructure;
using SampleForElastic.Infraustructure.Interceptors;

namespace SampleForElastic.Common
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

            return services;
        }
    }
}
