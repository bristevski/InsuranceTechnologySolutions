using Claims.Infrastructure.Audit;
using Claims.Infrastructure.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Claims.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void RegisterInfrastructureDependencies(IServiceCollection services, string sqlConnectionString, string mongoDBConnectionString, string mongoDbName)
        {
            services.AddScoped<IAuditContext, AuditContext>();
            services.AddScoped<IClaimsContext, ClaimsContext>();

            services.AddDbContext<AuditContext>(options => options.UseSqlServer(sqlConnectionString));
            services.AddDbContext<ClaimsContext>(
                options =>
                {
                    var client = new MongoClient(mongoDBConnectionString);
                    var database = client.GetDatabase(mongoDbName);
                    options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
                }
            );
        }
    }
}
