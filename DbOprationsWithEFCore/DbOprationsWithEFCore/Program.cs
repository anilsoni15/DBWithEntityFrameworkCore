
using DbOprationsWithEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DbOprationsWithEFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("AppDb")));

            builder.Services.AddLogging(log =>
            {
                log.AddSimpleConsole(Console.WriteLine);
            });
            // Add services to the container.
            //Add Coers
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseCors("AllowAngularApp");

            app.Run();
        }
    }
}
