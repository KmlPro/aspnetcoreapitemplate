using APITemplate._Infrastructure.Middleware;
using APITemplate.CQRS.MainInterface;
using APITemplate.CQRS.RegisterInstances;
using APITemplate.Model.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            #region Configure CQRS 
            services.RegisterCQRSInstances();

            services.AddTransient<ICQRS, CQRS.Main.CQRS>();

            #endregion Configure CQRS

            #region Configure DB 
            services.AddDbContextPool<APITemplateContext>(options =>
                    options.UseSqlServer(Configuration["APITemplateDB"]));
            #endregion Configure DB 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpStatusCodeExceptionMiddleware();

            UpdateDatabase(app);


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API template");
                c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<APITemplateContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
