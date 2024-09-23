using Microsoft.EntityFrameworkCore;
using WebApi.DbContexts;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PlatesDbContext>(o => o.UseSqlite(builder.Configuration["ConnectionStrings:PlatesDbConnectionString"]));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication().AddJwtBearer();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.RegisterPlatesEndpoints();
            
            app.Run();
        }
    }
}