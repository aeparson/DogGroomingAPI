using Grooming.Providers.Interfaces;
using Grooming.Providers.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Grooming.Providers
{
    /// <summary>
    /// This class provides configuration options for provider services.
    /// </summary>
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IPetProvider, PetProvider>();
            services.AddScoped<IAppointmentProvider, AppointmentProvider>();

            return services;
        }

    }
}
