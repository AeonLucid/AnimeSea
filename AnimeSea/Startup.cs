using AnimeSea.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeSea
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContextPool<SeaContext>(builder => builder.UseSqlite("./animesea.db"));
            services.AddSingleton<MetadataManager>();
            services.AddMvc();
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
