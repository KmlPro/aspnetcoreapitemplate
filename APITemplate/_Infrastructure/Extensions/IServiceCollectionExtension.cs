using APITemplate._Infrastructure.Commands;
using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.ICQRS;
using APITemplate._Infrastructure.Queries;
using APITemplate._Infrastructure.Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APITemplate._Infrastructure.Extension
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
            services.AddTransient<ICQRS.ICQRS, CQRS>();
        }      
    }
}
