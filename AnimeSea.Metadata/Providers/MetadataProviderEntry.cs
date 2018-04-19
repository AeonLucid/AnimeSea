using System;
using System.Reflection;

namespace AnimeSea.Metadata.Providers
{
    public class MetadataProviderEntry
    {
        private MetadataProviderBase _instance;

        public MetadataProviderEntry(MetadataProviderAttribute attribute, TypeInfo providerType)
        {
            Attribute = attribute;
            ProviderType = providerType;
        }

        public MetadataProviderAttribute Attribute { get; }

        public TypeInfo ProviderType { get; }

        public MetadataProviderBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (MetadataProviderBase) Activator.CreateInstance(ProviderType);
                }

                return _instance;
            }
        }
    }
}
