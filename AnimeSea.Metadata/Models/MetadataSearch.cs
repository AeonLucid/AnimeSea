namespace AnimeSea.Metadata.Models
{
    public class MetadataSearch
    {
        public MetadataSearchEntry[] Results { get; set; }

        /// <summary>
        ///     Passed to the primary metadata provider when the user
        ///     scrolls for more results. If set to null, "scroll for more results" will be
        ///     shown as unavailable to the user.
        /// </summary>
        public string Next { get; set; }
    }
}
