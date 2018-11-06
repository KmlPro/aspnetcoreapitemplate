using APITemplate.CQRS.Commands;
using APITemplate.CQRS.Commands.Interfaces;
using APITemplate.CQRS.Queries;
using APITemplate.CQRS.Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APITemplate.CQRS.RegisterInstances
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterCQRSInstances(this IServiceCollection services)
        {
            services.Scan(
                x =>
                {
                    var entryAssembly = Assembly.GetEntryAssembly();
                    var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                    var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.AssignableTo(typeof(ICommand)))
                        .AddClasses(classes => classes.AssignableTo(typeof(IQuery)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();

                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.Where(z => z.Name.EndsWith("QueryValidator")))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();


                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.Where(z => z.Name.EndsWith("QueryHandler")))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();

                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.Where(z => z.Name.EndsWith("CommandValidator")))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();


                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.Where(z => z.Name.EndsWith("CommandHandler")))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });

            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryBus, QueryBus>();
        }    
    }
}
