using System.Collections.Generic;

namespace AnimeSea.Metadata.Providers.Kitsu.Models.Attributes
{
    public class KitsuAnimeAttributes
    {
        public string Synopsis { get; set; }

        public Dictionary<string, string> Titles { get; set; }

        public string CanonicalTitle { get; set; }

        public string[] AbbreviatedTitles { get; set; }

        public KitsuAnimePosterImage PosterImage { get; set; }

        public KitsuAnimePosterImage CoverImage { get; set; }

        public int? EpisodeCount { get; set; }

        public int? EpisodeLength { get; set; }
    }
}
