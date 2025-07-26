using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;

namespace WebScrubbler.Parser
{
  public static class JSONParser
  {
    public static (IEnumerable<Scrobble> scrobbles, IEnumerable<string> errors) Parse(Stream fileStream, JSONParserConfiguration config, ScrobbleMode scrobbleMode = ScrobbleMode.Import)
    {
      var scrobbles = Enumerable.Empty<Scrobble>();
      var errors = new List<string>();
      var settings = new JsonSerializerSettings()
      {
        Converters = new List<JsonConverter> { new ScrobbleJsonConverter(config) },
        ContractResolver = new CustomJSONContractResolver(config),
        Error = delegate (object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        {
          errors.Add($"Object Number: {args.ErrorContext.Member} | Error: {args.ErrorContext.Error.Message}");
          args.ErrorContext.Handled = true;
        },
        NullValueHandling = NullValueHandling.Ignore
      };

      using (var reader = new StreamReader(fileStream))
      {
        scrobbles = JsonConvert.DeserializeObject<Scrobble[]>(reader.ReadToEnd(), settings) ?? throw new Exception("Unknown error parsing json file (corrupted?)");
      }

      if (config.FilterShortPlayedSongs)
      {
        var allowedTime = TimeSpan.FromMilliseconds(config.MillisecondsPlayedThreshold);
        scrobbles = scrobbles.Where(s => s.Duration == null || s.Duration >= allowedTime).ToArray();
      }

      return (scrobbles, errors);
    }
  }
}
