using AnimeSea.Data.Extensions;
using AnimeSea.Extensions;
using AnimeSea.Metadata;
using AnimeSea.Services;
using AnimeSea.Services.BackgroundTasks;
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

            // Metadata
            services.AddSingleton<MetadataManager>();

            // Database
            services.AddSingleton(provider => new BsonMapper().WithAnimeSeaMappings());

            services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var env = provider.GetRequiredService<IHostingEnvironment>();
                var mapper = provider.GetRequiredService<BsonMapper>();

                return new LiteDatabase(configuration.GetDatabasePath(env), mapper);
            });
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
