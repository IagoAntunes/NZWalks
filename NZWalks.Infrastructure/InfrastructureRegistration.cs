using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NZWalks.API.Data;
using NZWalks.API.Repositories.Implementations;
using NZWalks.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalks.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Configuração dos DbContexts
            services.AddDbContext<NZWalksDbContext>(
               options => options.UseSqlServer(
                   configuration.GetConnectionString("NZWalksConnectionString")
               )
            );

            services.AddDbContext<NZWalksAuthDbContext>(
               options => options.UseSqlServer(
                   configuration.GetConnectionString("NZWalksAuthConnectionString")
               )
            );

            // Registro dos Repositórios (Interfaces -> Implementações)
            // A camada de Infraestrutura conhece tanto a interface quanto a implementação.
            services.AddScoped<IRegionRepository, SQLRegionRepository>();
            services.AddScoped<IWalkRepository, SQLWalkRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IImageRepository, LocalImageRepository>();

            // Configuração do Identity
            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
                .AddEntityFrameworkStores<NZWalksAuthDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
