using Market.DTO.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WareHouse.Mapping;
using WareHouse.Models;
using WareHouse.Repositories.CategoryRepo;
using WareHouse.Repositories.ProductRepo;
using WareHouse.Repositories.StorageRepo;

namespace WareHouse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationBuilder? config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            IConfigurationRoot? cfg = config.Build();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IStorageRepository, StorageRepository>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                string server = "redishost";
                string port = "6379";
                string cnstring = $"{server}:{port}";
                options.Configuration = cnstring;
            });

            builder.Services.AddMemoryCache(options =>
            {
                options.TrackStatistics = true;
                options.TrackLinkedCacheEntries = true;
            });

            builder.Services.AddSingleton<Redis>();
            builder.Services.AddLogging();

            builder.Services.AddDbContext<WareHouseContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("db"));
            });

            string? connectionString = builder.Configuration.GetConnectionString("db");
            //builder.Services.AddDbContext<Testing2Context>(options => options.UseNpgsql(connectionString));

            //builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            //{
            //    containerBuilder.Register(c => new Testing2Context(connectionString)).InstancePerDependency();
            //});

            // Add services to the container.

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

            var staticFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilePath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilePath),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}