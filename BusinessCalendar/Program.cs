using DAL.Common;
using DAL.Context;
using Microsoft.Extensions.Options;

namespace BusinessCalendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");
            ConfigurationHelper.SetConfiguration(configBuilder.Build());

            builder.Services.AddControllers()
                            .AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ModelContext>(x =>
            {
                var connectionString = ContextHelper.BuildConnectionString(
                    ConfigurationHelper.GetString("serverName"),
                    ConfigurationHelper.GetInt("serverPort"),
                    ConfigurationHelper.GetString("databaseName"),
                    ConfigurationHelper.GetString("dbAdminLogin"),
                    ConfigurationHelper.GetString("dbAdminPassword")
                    );

                return new ModelContext(connectionString, 60);
            });
            
            builder.Services.AddScoped(x => new UnitOfWork());

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