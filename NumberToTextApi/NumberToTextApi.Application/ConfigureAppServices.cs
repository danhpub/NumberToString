using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NumberToTextApi.Application.Behaviours;
using NumberToTextApi.Application.Validators.Converter;
using System.Reflection;

namespace NumberToTextApi.Application
{
    public static class ConfigureAppServices
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection servces)
        {
            servces.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehaviourHandler<,>));
                }
            );
            servces.AddValidatorsFromAssemblyContaining(typeof(ConvertNumberQueryValidator));//one listed is enough
            return servces;
        }
    }
}
