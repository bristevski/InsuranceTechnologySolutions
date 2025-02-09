using System.Reflection;
using System.Text.Json.Serialization;
using Claims.Application;
using Claims.Infrastructure;
using Claims.Infrastructure.Audit;
using Claims.Infrastructure.Claims;
using Hangfire;
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

builder.Services.AddSingleton(TimeProvider.System);

InfrastructureModule.RegisterInfrastructureDependencies(builder.Services);
ApplicationModule.RegisterApplicationDependencies(builder.Services);

builder.Services.AddDbContext<AuditContext>(options => options.UseSqlServer(conf.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ClaimsContext>(
    options =>
    {
        var client = new MongoClient(conf.GetConnectionString("MongoDb"));
        var database = client.GetDatabase(conf["MongoDb:DatabaseName"]);
        options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
    }
);

// Configure Hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(conf.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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

// Enable the Hangfire dashboard
app.UseHangfireDashboard();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
    context.Database.Migrate();
}

app.Run();

public partial class Program { }
