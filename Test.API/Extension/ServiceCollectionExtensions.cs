using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Test.Application;
using Test.Infrastructure;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Test.API.Middlewares;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using System.Security.Cryptography.X509Certificates;


namespace Test.API.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            TokenValidationParameters tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = x509SecurityKey,

                ValidateIssuer = true,
                ValidIssuer = configuration.GetValue<string>("Authority:URL"),

                ValidateAudience = true,
                ValidAudiences = new[] { "Test" },

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(20),

                RequireExpirationTime = true,
                RequireSignedTokens = true,

                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                },

                ValidTypes = new[] { "at+jwt" },
                ValidAlgorithms = new[] { "RS256", "A128CBC-HS256" },

                //TokenDecryptionKey = decryptionCredentials
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = configuration.GetValue<string>("Authority:URL");
                o.RequireHttpsMetadata = configuration["ASPNETCORE_ENVIRONMENT"] != "Local";
                o.TokenValidationParameters = tokenValidationParameter;

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies["MB_IdentityServer_Token"] != null)
                        {
                            context.Token = context.Request.Cookies["MB_IdentityServer_Token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
        public static IServiceCollection ApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            return services;
        }

        public static IServiceCollection SwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test.API", Version = "1" });
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });

            return services;
        }

        public static IServiceCollection ForwardHeaders(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            return services;
        }

        //public static IServiceCollection AddServices(this IServiceCollection services)
        //{
        //    services.AddTransient<IJWTConfig, JWTConfig>();
        //    return services;
        //}
    }
}
