using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AnimeSea.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        ///     Get the path of the database.
        /// </summary>
        /// <param name="conf">The configuration.</param>
        /// <param name="env">The hosting environment.</param>
        /// <returns></returns>
        public static string GetDatabasePath(this IConfiguration conf, IHostingEnvironment env)
        {
            var path = conf.GetValue("db", "animesea.db");

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(env.ContentRootPath, path);
            }

            return path;
        }
    }
}
