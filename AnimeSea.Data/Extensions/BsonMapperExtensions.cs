using AnimeSea.Data.Entities;
using LiteDB;

namespace AnimeSea.Data.Extensions
{
    public static class BsonMapperExtensions
    {
        /// <summary>
        ///     Add the mappings of AnimeSea to the <see cref="BsonMapper"/>.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <returns><see cref="mapper"/></returns>
        public static BsonMapper WithAnimeSeaMappings(this BsonMapper mapper)
        {
            

            return mapper;
        }
    }
}
