using System.Collections.Generic;

namespace AnimeSea.Metadata.Providers.Kitsu.Models
{
    public class KitsuData<T>
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public KitsuLinksRelated Links { get; set; }

        public T Attributes { get; set; }

        public Dictionary<string, KitsuRelationship> Relationships { get; set; }
    }
}