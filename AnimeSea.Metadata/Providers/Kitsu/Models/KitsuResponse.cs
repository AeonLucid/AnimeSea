namespace AnimeSea.Metadata.Providers.Kitsu.Models
{
    public class KitsuResponse<T>
    {
        public string Id { get; set; }

        public KitsuData<T>[] Data { get; set; }

        public KitsuMeta Meta { get; set; }

        public KitsuLinks Links { get; set; }
    }
}
