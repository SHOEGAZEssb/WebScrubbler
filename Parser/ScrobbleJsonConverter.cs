using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebScrubbler.Parser
{
  public class ScrobbleJsonConverter : JsonConverter<Scrobble>
  {
    private readonly JSONParserConfiguration _config;

    public ScrobbleJsonConverter(JSONParserConfiguration config) 
    { 
      _config = config;
    }

    public override void WriteJson(JsonWriter writer, Scrobble? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override Scrobble? ReadJson(JsonReader reader, Type objectType, Scrobble? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JObject jo = JObject.Load(reader);
      var artist = jo[_config.ArtistNameProperty]?.Value<string>();
      var track = jo[_config.TrackNameProperty]?.Value<string>();
      var album = jo[_config.AlbumNameProperty]?.Value<string>();
      DateTimeOffset ts = default;
      if (jo.TryGetValue(_config.TimestampProperty, out var tsToken))
        ts =  new DateTimeOffset(tsToken.Value<DateTime>());
      string? albumArtist = null;
      if(jo.TryGetValue(_config.AlbumArtistNameProperty, out var albumArtistToken))
        albumArtist = albumArtistToken.Value<string>();
      TimeSpan? msPlayed = null;
      if (jo.TryGetValue(_config.MillisecondsPlayedProperty, out var msPlayedToken))
        msPlayed = TimeSpan.FromMilliseconds(msPlayedToken.Value<long>());

      return new Scrobble(artist, album, track, ts)
      {
        AlbumArtist = albumArtist,
        Duration = msPlayed
      };
    }
  }
}
