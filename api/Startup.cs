using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using HRApp.API.Helpers;
using HRApp.API.IServices;
using HRApp.API.Services;
using HRApp.API.Models;
using Npgsql;

namespace api
{
    public class Startup
    {

        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddCors("AllowAnyOrigin", builder => builder.AllowAnyOrigin());
            services.AddCors(options =>
            {
            options.AddPolicy( "AllowOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddControllers();

            string env = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(env))
            {
                var appSettingsSection = Configuration.GetSection("AppSettings");
                services.Configure<AppSettings>(appSettingsSection);
                var appSettings = appSettingsSection.Get<AppSettings>();
                env = appSettings.Secret;
            } 
            var key = Encoding.ASCII.GetBytes(env);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(30),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            string connStr;
            string databaseString = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrEmpty(databaseString))
            {
                // Use connection string from file.
                connStr = Configuration.GetConnectionString("DefaultConnection1");
            }
            else
            {
                var databaseUri = new Uri(databaseString);
                var userInfo = databaseUri.UserInfo.Split(':');

                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/')
                };

                connStr = builder.ToString();
            }

            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<UserContext>(options => options.UseNpgsql(connStr));
            services.AddDbContext<VisitorContext>(options => options.UseNpgsql(connStr));
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // app.UseMvc();

            app.UseRouting();

            app.UseCors("AllowOrigin");

            // Must be in sequence
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        

    }
}
