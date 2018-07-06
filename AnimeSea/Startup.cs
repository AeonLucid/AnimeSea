using AnimeSea.Data.Extensions;
using AnimeSea.Extensions;
using AnimeSea.Metadata;
using AnimeSea.Services;
using AnimeSea.Services.BackgroundTasks;
using Autofac;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeSea
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Metadata
            builder.RegisterType<MetadataManager>()
                .SingleInstance();

            // Database
            builder.RegisterType<BsonMapper>()
                .OnActivating(e => e.Instance.WithAnimeSeaMappings())
                .SingleInstance();

            builder.Register(c =>
                {
                    var configuration = c.Resolve<IConfiguration>();
                    var env = c.Resolve<IHostingEnvironment>();
                    var mapper = c.Resolve<BsonMapper>();

                    return new LiteDatabase(configuration.GetDatabasePath(env), mapper);
                })
                .SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
