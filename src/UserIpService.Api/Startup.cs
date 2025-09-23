using UserIpService.Api.Extensions;

namespace UserIpService.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration config, IWebHostEnvironment environment)
        {
            _config = config;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddUserIpJsonOptions();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddUserIpDb(_config)
                    .AddUserIpCore()
                    .AddUserIpMediatR()
                    .AddUserIpValidation();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
