using Microsoft.Extensions.Configuration;
using AnimeSea.Data.Extensions;
using AnimeSea.Providers;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NLog;

namespace AnimeSea.Extensions
{
    public static class ConigurationBuilderExtensions
    {
        /// <summary>
        ///     Add the defaults of AnimeSea to the configuration builder.
        ///
        ///     This is used for <see cref="LiteDatabaseConfigSource"/>.
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

        /// <summary>
        ///     Add the database configuration to the configuration builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="env"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static IConfigurationBuilder AddLiteDatabaseConfig(this IConfigurationBuilder builder,
            IHostingEnvironment env, string[] args)
        {
            var conf = new ConfigurationBuilder()
                .AddDefaults(env, args, reloadOnChange: false)
                .Build();

            var userDbConfig = conf.GetValue("useDbConfig", true);

            LogManager.GetCurrentClassLogger().Debug($"Using database configuration: {userDbConfig}");

            if (userDbConfig)
            {
                builder.Add(new LiteDatabaseConfigSource(() => new LiteDatabase(
                    conf.GetDatabasePath(env),
                    new BsonMapper().WithAnimeSeaMappings()
                )));
            }

            return builder;
        }
    }
}
