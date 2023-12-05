using BusinessCalendar.Helpers;
using DAL.Common;
using DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BusinessCalendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationHelper.SetConfiguration(builder.Configuration);
            ImageFileHelper.InitConfiguration();
            AuthHelper.InitConfiguration();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = AuthHelper.Issuer,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthHelper.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                }
            );

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