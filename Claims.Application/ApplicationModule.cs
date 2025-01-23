using Claims.Application.Interfaces;
using Claims.Application.Providers;
using Claims.Application.Services;
using Claims.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application;

public static class ApplicationModule
{
    public static void RegisterApplicationDependencies(IServiceCollection services)
    {
        services.AddScoped<IGuidProvider, GuidProvider>();
        services.AddScoped<IComputingStrategyProvider, ComputingStrategyProvider>();

        services.AddScoped<ICoverValidator, CoverValidator>();
        services.AddScoped<IClaimValidator, ClaimValidator>();

        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<ICoverService, CoverService>(); 
    }
}
