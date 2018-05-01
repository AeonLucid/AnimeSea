using System.IO;
using AnimeSea.Data;
using AnimeSea.Metadata;
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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MetadataManager>()
                .SingleInstance();

            builder.RegisterType<BsonMapper>()
                .OnActivating(e => SeaDatabase.Prepare(e.Instance))
                .SingleInstance();

            builder.Register(c =>
                {
                    var conf = c.Resolve<IConfiguration>();
                    var mapper = c.Resolve<BsonMapper>();
                    var path = conf.GetValue("db", "animesea.db");

                    if (!Path.IsPathRooted(path))
                    {
                        path = Path.Combine(c.Resolve<IHostingEnvironment>().ContentRootPath, path);
                    }

                    return new LiteDatabase(path, mapper);
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
