using BusinessCalendar.Helpers;
using DAL.Common;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BusinessCalendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationHelper.SetConfiguration(builder.Configuration);
            ImageFileHelper.InitConfiguration();
           
            builder.Services.AddControllers().AddNewtonsoftJson(jsonOptions =>
            {
                jsonOptions.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    ConfigurationHelper.GetString("MajorVersion"),
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Business Calendar API",
                        Version = ConfigurationHelper.GetString("FullVersion")
                    }
                );
            });

            builder.Services.AddDbContext<ModelContext>(options =>
            {
                options.UseNpgsql(GetConnectionString());
            });

            builder.Services.AddScoped<ModelContext>(_ => new ModelContext(GetConnectionString()));

            builder.Services.AddScoped(_ => new UnitOfWork());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/{ConfigurationHelper.GetString("MajorVersion")}/swagger.json", ConfigurationHelper.GetString("MajorVersion"));
                    // c.SwaggerEndpoint($"/swagger/{ConfigurationHelper.GetString("MajorVersion")}/swagger.json", ConfigurationHelper.GetString("MajorVersion"));
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static string GetConnectionString()
        {
            return ContextHelper.BuildConnectionString(
                    ConfigurationHelper.GetString("dbServerHost"),
                    ConfigurationHelper.GetInt("dbServerPort"),
                    ConfigurationHelper.GetString("dbName"),
                    ConfigurationHelper.GetString("dbAdminLogin"),
                    ConfigurationHelper.GetString("dbAdminPassword")
                    );
        }
    }
}