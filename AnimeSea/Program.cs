using System;
using System.IO;
using AnimeSea.Config;
using AnimeSea.Extensions;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;

namespace AnimeSea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Configuration = NLogConfig.Create();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            new WebHostBuilder()
                .UseKestrel((builderContext, options) => options.Configure(builderContext.Configuration.GetSection("Kestrel")))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddDefaults(env, args);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()
                .ConfigureServices((hostingContext, services) =>
                {
                    // Fallback
                    services.PostConfigure<HostFilteringOptions>(options =>
                    {
                        // "AllowedHosts": "localhost;127.0.0.1;[::1]"
                        var hosts = hostingContext.Configuration["AllowedHosts"]?.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                        // Fall back to "*" to disable.
                        options.AllowedHosts = hosts?.Length > 0 ? hosts : new[] {"*"};
                    });

                    // Change notification
                    services.AddSingleton<IOptionsChangeTokenSource<HostFilteringOptions>>(
                        new ConfigurationChangeTokenSource<HostFilteringOptions>(hostingContext.Configuration)
                    );
                })
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
    }
}
