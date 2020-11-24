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
            // string databaseString = "postgres://qqylwlibbqzfnt:4712d6c25092370e4e5fc34286c1c15c143a1039ec52562b7d79d51731cbf483@ec2-34-232-147-86.compute-1.amazonaws.com:5432/d97hs6fcue95i0";
            if (string.IsNullOrEmpty(databaseString))
            {
                // Use connection string from file.
                connStr = Environment.GetEnvironmentVariable("DefaultConnection1");
            }
            else
            {

                // var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var databaseUri = new Uri(databaseString);
                Console.WriteLine("*******************");
                Console.WriteLine(databaseUri);
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
                
                
                // Parse connection URL to connection string for Npgsql
                // databaseString =  databaseString.Replace("postgres://", string.Empty);
                // var pgUserPass =    databaseString.Split("@")[0];
                // var pgHostPortDb =  databaseString.Split("@")[1];
                // var pgHostPort = pgHostPortDb.Split("/")[0];
                // var pgDb = pgHostPortDb.Split("/")[1];
                // var pgUser = pgUserPass.Split(":")[0];
                // var pgPass = pgUserPass.Split(":")[1];
                // var pgHost = pgHostPort.Split(":")[0];
                // var pgPort = pgHostPort.Split(":")[1];

                // connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}";
                // "Server=ec2-34-232-147-86.compute-1.amazonaws.com;"
                // "Port=5432;Username=qqylwlibbqzfnt;"
                // "Password=4712d6c25092370e4e5fc34286c1c15c143a1039ec52562b7d79d51731cbf483;"
                // "Database=d97hs6fcue95i0;"
                // "SslMode=Require;"
                // "TrustServerCertificate=true;"

                
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
