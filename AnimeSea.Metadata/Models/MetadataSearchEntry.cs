using System.Collections.Generic;

namespace AnimeSea.Metadata.Models
{
    public class MetadataSearchEntry
    {
        public string Title { get; set; }

        public Dictionary<string, string> Titles { get; set; }

        public string[] Genres { get; set; }

        public string Synopsis { get; set; }

        public string Image { get; set; }

        public int? EpisodeCount { get; set; }

        public int? EpisodeLength { get; set; }
    }
}