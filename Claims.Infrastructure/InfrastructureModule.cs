using Claims.Infrastructure.Audit;
using Claims.Infrastructure.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Claims.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void RegisterInfrastructureDependencies(IServiceCollection services)
        {
            services.AddScoped<IAuditContext, AuditContext>();
            services.AddScoped<IClaimsContext, ClaimsContext>();
            services.AddScoped<IClaimsUnitOfWork, ClaimsUnitOfWork>();
        }
    }
}
