namespace AnimeSea.Data.Entities
{
    public class Serie
    {
        /// <summary>
        ///     The unique ID of the serie.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The title of the serie.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The provider ID.
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        ///     The ID of the serie of the provider.
        /// </summary>
        public string ProviderSerieId { get; set; }
    }
}
