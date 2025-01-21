using System.Text.Json.Serialization;
using Claims.Application;
using Claims.Application.Interfaces;
using Claims.Application.Providers;
using Claims.Application.Services;
using Claims.Application.Validators;
using Claims.Infrastructure;
using Claims.Infrastructure.Audit;
using Claims.Infrastructure.Claims;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var conf = builder.Configuration;

//test123(builder, conf);
//ApplicationModule.RegisterApplicationDependencies(builder.Services);
//InfrastructureModule.RegisterInfrastructureDependencies(builder.Services, 
//                                    conf.GetConnectionString("DefaultConnection"), 
//                                    conf.GetConnectionString("MongoDb"),
//                                    conf["MongoDb:DatabaseName"]);

//START

//builder.Services.AddScoped<IAuditContext, AuditContext>();
//builder.Services.AddScoped<IClaimsContext, ClaimsContext>();

InfrastructureModule.RegisterInfrastructureDependencies(builder.Services);
ApplicationModule.RegisterApplicationDependencies(builder.Services);

//builder.Services.AddScoped<IGuidProvider, GuidProvider>();
//builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
//builder.Services.AddScoped<IComputingStrategyProvider, ComputingStrategyProvider>();

//builder.Services.AddScoped<ICoverValidator, CoverValidator>();
//builder.Services.AddScoped<IClaimValidator, ClaimValidator>();

//builder.Services.AddScoped<IAuditService, AuditService>();
//builder.Services.AddScoped<IClaimService, ClaimService>();
//builder.Services.AddScoped<ICoverService, CoverService>();

builder.Services.AddDbContext<AuditContext>(options => options.UseSqlServer(conf.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ClaimsContext>(
    options =>
    {
        var client = new MongoClient(conf.GetConnectionString("MongoDb"));
        var database = client.GetDatabase(conf["MongoDb:DatabaseName"]);
        options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
    }
);

//END
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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
    context.Database.Migrate();
}

app.Run();

//static void test123(WebApplicationBuilder builder, ConfigurationManager conf)
//{
//    builder.Services.AddDbContext<AuditContext>(options => options.UseSqlServer(conf.GetConnectionString("DefaultConnection")));
//    builder.Services.AddDbContext<ClaimsContext>(
//        options =>
//        {
//            var client = new MongoClient(conf.GetConnectionString("MongoDb"));
//            var database = client.GetDatabase(conf["MongoDb:DatabaseName"]);
//            options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
//        }
//    );
//}

public partial class Program { }
