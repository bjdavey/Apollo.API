using Apollo.API.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace Apollo.API
{
    public class Program
    {
        public static ConfigurationManager AppSettings;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
            AppSettings = builder.Configuration;

            var IssuerSigningKey = configuration.GetValue<string>("IssuerSigningKey");
            if (IssuerSigningKey != null)
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtBearerOptions =>
                    {
                        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateActor = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "Issuer",
                            ValidAudience = "Audience",
                            SaveSigninToken = true,
                            RequireExpirationTime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey))
                        };
                    });

            //builder.Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromDays(1);
            //});
            builder.Services.AddMemoryCache();
            builder.Services.AddCors();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //builder.Services.AddScoped<IOrderService, OrderService>();

            var ConnectionString = configuration.GetValue<string>("ConnectionString");
            if (ConnectionString != null)
                builder.Services.AddDbContext<ApolloContext>(options =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        options.UseMySql(ConnectionString, ServerVersion.Parse("8.0.37-mysql"))
                               .LogTo(message => Debug.WriteLine(message));
                    }
                    else
                    {
                        options.UseMySql(ConnectionString, ServerVersion.Parse("8.0.37-mysql"));
                    }
                });

            var app = builder.Build();

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var result = JsonConvert.SerializeObject(new
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Message = exception?.Message,
                    InnerException = (exception?.InnerException != null) ? exception?.InnerException?.Message : null
                });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseCors(builder => builder
                               .AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader());

            //app.UseSession();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = "/Files"
            });

            app.MapControllers();

            app.Run();

        }
    }
}
