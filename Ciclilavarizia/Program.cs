using AuthLibrary;
using AuthLibrary.Models;
using Ciclilavarizia.Data;
using Ciclilavarizia.Exceptions;
using Ciclilavarizia.Models;
using Ciclilavarizia.Services.CustomerService;
using Ciclilavarizia.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

namespace Ciclilavarizia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionStringProd = builder.Configuration.GetConnectionString("AdventureWorks")
                    ?? throw new InvalidOperationException("Connection string not found");

            var connectionStringSecurity = builder.Configuration.GetConnectionString("AdventureWorksSecurity")
                    ?? throw new InvalidOperationException("Connection string not found");

            //Add Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("Logs\\ciclilavarizia.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add services to the container.

            // Add SQL Server Context
            builder.Services.AddDbContext<CiclilavariziaContext>(options =>
            {
                options.UseSqlServer(connectionStringProd);
            });

            // Add Mongo DB
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<MongoDbService>();

            // Add Middleware Exception
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Add Auth Library 
            var tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>()
                ?? throw new InvalidOperationException("Token settings not found");

            SqlService sqlService = new(connectionStringProd, connectionStringSecurity, tokenSettings);
            builder.Services.AddSingleton(sqlService);

            //Authroization
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   var key = System.Text.Encoding.ASCII.GetBytes(tokenSettings.SecretKey!);
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = tokenSettings.Issuer,
                       ValidAudience = tokenSettings.Audience,
                       ClockSkew = TimeSpan.FromSeconds(3),
                       IssuerSigningKey = new SymmetricSecurityKey(key)
                   };
               });


            // My services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapSwagger("/openapi/{documentName}.json");
                app.MapScalarApiReference();
            }

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
