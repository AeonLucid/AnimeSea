using System;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace AnimeSea.Providers
{
    public class LiteDatabaseConfigSource : IConfigurationSource
    {
        private readonly Func<LiteDatabase> _databaseFunc;

        public LiteDatabaseConfigSource(Func<LiteDatabase> databaseFunc)
        {
            _databaseFunc = databaseFunc;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new LiteDatabaseConfigProvider(_databaseFunc);
        }
    }
}
