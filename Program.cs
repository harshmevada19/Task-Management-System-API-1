using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Repositories;

namespace Task_Management_System_API_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ensure the JWT key is set
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new Exception("JWT key is not configured in appsettings.json");
            }

            var key = Encoding.UTF8.GetBytes(jwtKey);

            // Configure Authentication and JWT Bearer middleware
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero // Ensures tokens expire exactly at token expiration time
                };
            });

            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders()
                    .AddApiEndpoints();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ViewDataPolicy", policy =>
                    policy.RequireClaim("Permission", "ViewData")
                          .RequireRole("Admin", "Manager"));
            });


            // Add DbContext for ApplicationDbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TaskDBLocalConnection")));

            // Register AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Add Services
            builder.Services.AddTransient<IGenericRepository<Task>, GenericRepository<Task>>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            // Add Controllers
            builder.Services.AddControllers();

            // Add Swagger with JWT Bearer token support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management System API 1 APIs", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter the token with 'Bearer ' prefix",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                }); 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Build the application
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // This will provide detailed error messages during development
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // Forwarded Headers Configuration
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Map Identity API endpoints
            app.MapIdentityApi<IdentityUser>();

            // Map Controllers
            app.MapControllers();

            // Run the application
            app.Run();

        }
    }
}
