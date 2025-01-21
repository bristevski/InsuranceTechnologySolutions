using Claims.Application.Interfaces;
using Claims.Application.Providers;
using Claims.Application.Services;
using Claims.Core.Audit.Interfaces;
using Claims.Core.Claims.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application
{
    public static class ApplicationModule
    {
        public static void RegisterApplicationDependencies(IServiceCollection services)
        {
            services.AddScoped<IGuidProvider, GuidProvider>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IComputingStrategyProvider, ComputingStrategyProvider>();

            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<ICoverService, CoverService>(); 
        }
    }
}
