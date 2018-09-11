using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using APITemplate._Infrastructure.Commands;
using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.Middleware;
using APITemplate._Infrastructure.Queries;
using APITemplate._Infrastructure.Queries.Interfaces;
using APITemplate._Infrastructure.Validator.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace APITemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "APITemplate", Version = "v1", Description = "Simple template with CQRS patterns" });
            });

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpStatusCodeExceptionMiddleware();
            }
            else
            {
                app.UseHttpStatusCodeExceptionMiddleware();
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API template");
                c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
