using System;
using System.Threading.Tasks;
using AnimeSea.Metadata.Models;

namespace AnimeSea.Metadata.Providers
{
    public abstract class MetadataProviderBase
    {
        /// <summary>
        ///     Whether this metadata provider can be used to search for anime.
        /// </summary>
        public virtual bool CanSearch { get; } = false;

        /// <summary>
        ///     Whether this metadata provider can be used as a primary provider.
        /// </summary>
        public bool CanBePrimary => CanSearch;

        public virtual Task<MetadataSearch> SearchAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}
