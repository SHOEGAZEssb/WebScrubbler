using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Serialization;

namespace WebScrubbler.Parser
{
  /// <summary>
  /// Custom json property resolver.
  /// </summary>
  class CustomJSONContractResolver : DefaultContractResolver
  {
    /// <summary>
    /// Dictionary containing the mapped properties.
    /// </summary>
    private readonly Dictionary<string, string> _propertyMappings;

    private readonly JSONParserConfiguration _config;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CustomJSONContractResolver(JSONParserConfiguration config)
    {
      _config = config;

      _propertyMappings = new Dictionary<string, string>()
      {
        { nameof(Scrobble.Track), config.TrackNameProperty },
        { nameof(Scrobble.Artist), config.ArtistNameProperty },
        { nameof(Scrobble.Album), config.AlbumNameProperty },
        { nameof(Scrobble.AlbumArtist), config.AlbumArtistNameProperty },
        { nameof(Scrobble.TimePlayed), config.TimestampProperty },
        { nameof(Scrobble.Duration), config.MillisecondsPlayedProperty }
      };
    }

    /// <summary>
    /// Resolves the name of the property with the given <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="propertyName">Name of the property to resolve.</param>
    /// <returns>Resolved name.</returns>
    protected override string ResolvePropertyName(string propertyName)
    {
      return _propertyMappings.TryGetValue(propertyName, out string? resolvedName) ? resolvedName : base.ResolvePropertyName(propertyName);
    }
  }
}