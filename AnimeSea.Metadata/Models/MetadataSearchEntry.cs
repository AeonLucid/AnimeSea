using System.Collections.Generic;

namespace AnimeSea.Metadata.Models
{
    public class MetadataSearchEntry
    {
        /// <summary>
        ///     Required: Title of the show or movie.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Optional: Alternative titles.
        /// </summary>
        public Dictionary<string, string> Titles { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string[] Genres { get; set; }

        /// <summary>
        ///     Optional, but recommended!
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        ///     Optional, but recommended!
        /// </summary>
        public string Image { get; set; }
        
        /// <summary>
        ///     Required.
        /// </summary>
        public int? EpisodeCount { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public int? EpisodeLength { get; set; }
    }
}