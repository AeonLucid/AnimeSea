using System;

namespace AnimeSea.Metadata.Providers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MetadataProviderAttribute : Attribute
    {
        /// <summary>
        ///     Defines a implementation of <see cref="MetadataProviderBase"/>.
        /// </summary>
        /// <param name="id">The unique identifier of this metadata provider. It must never change!</param>
        /// <param name="name">
        ///     The name of this metadata provider, preferably the name
        ///     of the website it gathers data from.
        /// </param>
        public MetadataProviderAttribute(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        ///     The unique identifier of this metadata provider. It must never change!
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     The website name where this metadata provider gathers data from.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The website url where this metadata provider gathers data from.
        /// </summary>
        public string Website { get; set; }
    }
}
