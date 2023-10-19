using DAL.Common;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace BusinessCalendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationHelper.SetConfiguration(builder.Configuration);

            builder.Services.AddControllers().AddNewtonsoftJson(jsonOptions =>
            {
                jsonOptions.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ModelContext>(options =>
            {
                options.UseNpgsql(GetConnectionString());
            });

            builder.Services.AddScoped<ModelContext>(x =>
            {
                return new ModelContext(GetConnectionString());
            });

            builder.Services.AddScoped(x => new UnitOfWork());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, ConfigurationHelper.GetString("imagesPath"))),
                RequestPath = "/images"
            });

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