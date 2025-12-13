using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register AutoMapper profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ExerciseProfile).Assembly);

            });

            services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
           );

            return services;
        }
    }
}
