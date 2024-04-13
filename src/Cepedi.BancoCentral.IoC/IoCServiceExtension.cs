using System.Diagnostics.CodeAnalysis;
using Cepedi.BancoCentral.Data;
using Cepedi.BancoCentral.Data.Repositories;
using Cepedi.BancoCentral.Domain.Handlers.Pipelines;
using Cepedi.BancoCentral.Domain.Repository;
using Cepedi.BancoCentral.Shareable;
using Cepedi.BancoCentral.Shareable.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cepedi.BancoCentral.IoC
{
    [ExcludeFromCodeCoverage]
    public static class IoCServiceExtension
    {
        public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDbContext(services, configuration);
            services.AddMediatR(config =>
                     config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            );
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //services.AddTransient<ExceptionHandlingMiddleware>();
            //services.AddValidatorsFromAssembly(typeof(EntryPoint).Assembly);
            ConfigurarFluentValidation(services);

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            //services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
        }

        private static void ConfigurarFluentValidation(IServiceCollection services)
        {
            var abstractValidator = typeof(AbstractValidator<>);
            var validadores = typeof(QualquerCoisa)
                .Assembly
                .DefinedTypes
                .Where(type => type.BaseType?.IsGenericType is true &&
                type.BaseType.GetGenericTypeDefinition() ==
                abstractValidator)
                .Select(Activator.CreateInstance)
                .ToArray();

            foreach (var validator in validadores)
            {
                services.AddSingleton(validator!.GetType().BaseType!, validator);
            }
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                //options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ApplicationDbContextInitialiser>();
        }
    }
}
