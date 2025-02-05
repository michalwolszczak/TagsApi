using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text.Json.Serialization;
using TagsApi.Common;
using TagsApi.Extensions;
using TagsApi.Middlewares;
using TagsApi.Services;

namespace TagsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.Configure<TagDatabaseSettings>(
                builder.Configuration.GetSection("TagDatabase"));

            //configure MongoDb
            var tagDatabaseSettings = builder.Configuration.GetSection("TagDatabase").Get<TagDatabaseSettings>();
            ArgumentNullException.ThrowIfNull(tagDatabaseSettings);
            builder.Services.ConfigureMongoDb(tagDatabaseSettings);


            //init data
            builder.Services.AddHostedService<InitializationHostedService>();

            builder.Services.AddHttpClient("StackExchangeHttpClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.stackexchange.com");
            });
            builder.Services.AddTransient<ExceptionLoggerMiddleware>();
            builder.Services.AddTransient<IProblemDetailsFactory, ProblemDetailsFactory>();
            builder.Services.AddTransient<ITagService, TagService>();
            builder.Services.AddTransient<IInitializeService, InitializeService>();
            builder.Services.AddTransient<ITagSharedCalculationService, TagSharedCalculationService>();
            builder.Services.AddSingleton<ITagRepoService, TagRepoService>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.EnableAnnotations();                
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionLoggerMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
