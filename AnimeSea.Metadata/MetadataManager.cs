using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnimeSea.Metadata.Providers;

namespace AnimeSea.Metadata
{
    public class MetadataManager
    {
        public MetadataManager()
        {
            MetadataProviders = new Dictionary<int, MetadataProviderEntry>();

            Initialize();
        }

        public Dictionary<int, MetadataProviderEntry> MetadataProviders { get; }

        /// <summary>
        ///     Initializes the <see cref="MetadataManager"/> by searching for
        ///     metadata providers that use <see cref="MetadataProviderAttribute"/>.
        /// </summary>
        private void Initialize()
        {
            var providers = typeof(MetadataManager).Assembly
                .DefinedTypes
                .Where(x => x.GetCustomAttribute<MetadataProviderAttribute>() != null)
                .ToArray();

            foreach (var provider in providers)
            {
                var providerAttribute = provider.GetCustomAttribute<MetadataProviderAttribute>();

                MetadataProviders.Add(providerAttribute.Id, new MetadataProviderEntry(providerAttribute, provider));
            }
        }
    }
}
