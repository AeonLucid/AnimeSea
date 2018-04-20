using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeSea.Metadata.Models;
using AnimeSea.Metadata.Providers.Kitsu.Models;
using AnimeSea.Metadata.Providers.Kitsu.Models.Attributes;
using Flurl.Http;

namespace AnimeSea.Metadata.Providers.Kitsu
{
    [MetadataProvider(0, "Kitsu", Website = "https://kitsu.io/")]
    public class KitsuProvider : MetadataProviderBase
    {
        private readonly FlurlClient _client;

        /// <summary>
        ///     Implemented by using https://kitsu.docs.apiary.io/.
        /// </summary>
        public KitsuProvider()
        {
            _client = new FlurlClient("https://kitsu.io/api/edge");
        }

        public override async Task<MetadataSearch> SearchAsync(string query)
        {
            // Request anime entries from kitsu.
            var response = await "anime"
                .WithClient(_client)
                .SetQueryParam("filter[text]", query)
                .GetJsonAsync<KitsuResponse<KitsuAnimeAttributes>>();

            // Parse to our desired format.
            var result = new List<MetadataSearchEntry>();

            foreach (var animeData in response.Data)
            {
                var attr = animeData.Attributes;
                string[] genres = null;

                if (animeData.Relationships.ContainsKey("genres"))
                {
                    // Gotta do an extra request to get genres for an anime.
                    // It's pretty slow as well so we might as well disable it.

                    // genres = (await animeData.Relationships["genres"].Links.Related
                    //         .WithClient(_client)
                    //         .GetJsonAsync<KitsuResponse<KitsuAnimeGenresAttributes>>()).Data
                    //     .Select(x => x.Attributes.Name)
                    //     .ToArray();
                }

                result.Add(new MetadataSearchEntry
                {
                    Title = attr.CanonicalTitle,
                    Titles = attr.Titles,
                    Genres = genres,
                    PosterImage = attr.PosterImage != null
                        ? attr.PosterImage.Original
                          ?? attr.PosterImage.Large
                          ?? attr.PosterImage.Medium
                          ?? attr.PosterImage.Small
                          ?? attr.PosterImage.Tiny
                        : null,
                    CoverImage = attr.CoverImage != null
                        ? attr.CoverImage.Medium
                          ?? attr.CoverImage.Large
                          ?? attr.CoverImage.Original
                          ?? attr.CoverImage.Small
                          ?? attr.CoverImage.Tiny
                        : null,
                    Synopsis = attr.Synopsis,
                    EpisodeCount = attr.EpisodeCount,
                    EpisodeLength = attr.EpisodeLength
                });
            }

            return new MetadataSearch
            {
                Results = result.ToArray(),
                Next = response.Links.Next
            };
        }
    }
}
