using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserIpService.Application;
using UserIpService.Application.Behaviors;
using UserIpService.Application.Commands.ProcessConnection;
using UserIpService.Application.Queries.FindUsersByIpPrefix;
using UserIpService.Application.Queries.GetLastConnectionByIp;
using UserIpService.Application.Queries.GetUserIps;
using UserIpService.Application.Queries.GetUserLastConnection;
using UserIpService.Core.Converters;
using UserIpService.Core.Interfaces;
using UserIpService.Core.Services;
using UserIpService.Infrastructure.Data;
using UserIpService.Infrastructure.Repositories;

namespace UserIpService.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserIpDb(this IServiceCollection services, IConfiguration config)
        {
            var connStr = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Ошибка! Строка подключения к БД не найдена.");
            services.AddDbContext<UserIpContext>(options => options.UseNpgsql(connStr));

            return services;
        }

        public static IServiceCollection AddUserIpCore(this IServiceCollection services)
        {
            services.AddScoped<IUserIpRepository, UserIpRepository>();
            services.AddScoped<IUserConnectionService, UserConnectionService>();

            return services;
        }

        public static IServiceCollection AddUserIpMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(UserConnectionService).Assembly,
                    typeof(UserIpRepository).Assembly,
                    typeof(ApplicationMarker).Assembly
                );
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddUserIpValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<ProcessConnectionCommandValidator>()
                .AddValidatorsFromAssemblyContaining<FindUsersByIpPrefixQueryValidator>()
                .AddValidatorsFromAssemblyContaining<GetLastConnectionDateTimeByIpQueryValidator>()
                .AddValidatorsFromAssemblyContaining<GetUserIpsByUserIdQueryValidator>()
                .AddValidatorsFromAssemblyContaining<GetUserLastConnectionByUserIdQueryValidator>();           
           
            return services;
        }

        public static IMvcBuilder AddUserIpJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new IPAddressConverter());
            });

            return builder;
        }
    }
}
