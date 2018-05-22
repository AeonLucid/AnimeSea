using System;
using System.Linq;
using AnimeSea.Data.Entities;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace AnimeSea.Providers
{
    public class LiteDatabaseConfigProvider : ConfigurationProvider
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     The keys that should be ignored.
        /// </summary>
        public static readonly string[] IgnoreKeys = {"db", "useDbConfig"};

        private readonly Func<LiteDatabase> _databaseFunc;

        public LiteDatabaseConfigProvider(Func<LiteDatabase> databaseFunc)
        {
            _databaseFunc = databaseFunc;
        }

        public override void Load()
        {
            using (var database = _databaseFunc())
            {
                // Load the values
                Data = database.GetCollection<ConfigurationValue>()
                    .FindAll()
                    .ToDictionary(c => c.Id, c => c.Value);

                // Skip the ignore keys
                var invalidKeys = Data.Keys
                    .Where(key => IgnoreKeys.Contains(key))
                    .ToArray();

                if (invalidKeys.Length <= 0)
                {
                    return;
                }

                foreach (var key in invalidKeys)
                {
                    Data.Remove(key);
                }

                Logger.Warn($"Ignored database configuration: {string.Join(", ", invalidKeys)}");
            }
        }
    }
}
