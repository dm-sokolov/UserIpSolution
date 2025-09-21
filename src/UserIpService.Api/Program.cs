
using Microsoft.EntityFrameworkCore;
using UserIpService.Core.Converters;
using UserIpService.Core.Interfaces;
using UserIpService.Core.Services;
using UserIpService.Infrastructure;
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

            builder.Services.AddDbContext<UserIpContext>(options => options.UseNpgsql(connStr));

            builder.Services.AddScoped<IUserIpRepository, UserIpRepository>();
            builder.Services.AddScoped<IUserConnectionService, UserConnectionService>();

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
