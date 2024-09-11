using FinalDern_Support.Data;
using FinalDern_Support.Models;
using FinalDern_Support.Repositories.Interfaces;
using FinalDern_Support.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FinalDern_Support
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var ConnectionStringVar = builder.Configuration.GetConnectionString("DefaultConnection");


            // DbContext Configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(ConnectionStringVar));
            // Identity configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // Service Registrations
            builder.Services.AddControllers();
            builder.Services.AddScoped<Jwt_TokenServices>();
            builder.Services.AddTransient<IUser, IdentityUserServices>();
            builder.Services.AddTransient<ICustomer, CustomerService>();
            builder.Services.AddScoped<IAdmin, AdminService>();

            // builder.Services.AddTransient<IHome, HomeServices>();

            // Swagger Configuration
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Dern-Support API",
                    Version = "v1",
                    Description = "API for Technician Support"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter user token below."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            //Add auth service to the app using jwt 
            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = Jwt_TokenServices.ValidateToken(builder.Configuration);
                    }
                );

            var app = builder.Build();

            // Middleware Configuration
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Swagger Middleware
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/v1/swagger.json", "Dern-Support API v1");
                options.RoutePrefix = "";
            });


            app.Run();
        }
    }
}
