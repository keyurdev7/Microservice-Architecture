using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Persistence.Blog;
using Test.Application.Persistence.User;
using Test.Infrastructure.Persistence;
using Test.Infrastructure.Repositories.Blog;
using Test.Infrastructure.Repositories.User;

namespace Test.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IUserRespository, UserRespository>();

            return services;
        }
    }
}
