using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AnimeSea.Extensions
{
    public static class ConigurationBuilderExtensions
    {
        /// <summary>
        ///     Add the defaults of AnimeSea to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="reloadOnChange">True if the configuration should be reloaded on change.</param>
        /// <returns></returns>
        internal static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder, IHostingEnvironment env,
            string[] args, bool reloadOnChange = true)
        {
            return builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
        }
    }
}
