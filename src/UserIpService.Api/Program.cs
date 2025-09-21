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

namespace UserIpService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new IPAddressConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddDbContext<UserIpContext>(options =>
                options.UseNpgsql(connStr));


            builder.Services.AddScoped<IUserIpRepository, UserIpRepository>();
            builder.Services.AddScoped<IUserConnectionService, UserConnectionService>();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(UserConnectionService).Assembly,
                    typeof(UserIpRepository).Assembly,
                    typeof(ApplicationMarker).Assembly
                );
            });

            builder.Services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<ProcessConnectionCommandValidator>()
                .AddValidatorsFromAssemblyContaining<FindUsersByIpPrefixQueryValidator>()
                .AddValidatorsFromAssemblyContaining<GetLastConnectionByIpQueryValidator>()
                .AddValidatorsFromAssemblyContaining<GetUserIpsQueryValidator>()
                .AddValidatorsFromAssemblyContaining<GetUserLastConnectionQueryValidator>()
                ;

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
