using IF.Lastfm.Core.Objects;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Text;

namespace WebScrubbler.Parser
{
  public enum ScrobbleMode
  {
    Normal,
    Import
  }

  /// <summary>
  /// Parses a .csv file.
  /// </summary>
  class CSVParser
  {
    /// <summary>
    /// Different formats to try in case TryParse fails.
    /// </summary>
    private static readonly string[] _formats = ["M/dd/yyyy h:mm"];

    /// <summary>
    /// Parses the given <paramref name="file"/>.
    /// </summary>
    /// <param name="file">File to parse.</param>
    /// <param name="defaultDuration">Default duration for tracks.</param>
    /// <param name="scrobbleMode"></param>
    /// <returns>Parse result.</returns>
    public static (IEnumerable<Scrobble> scrobbles, IEnumerable<string> errors) Parse(Stream fileStream, CSVParserConfiguration config, ScrobbleMode scrobbleMode = ScrobbleMode.Import)
    {
      var scrobbles = new List<Scrobble>();
      var errors = new List<string>();
      string[] fields = null;

      using (var parser = new TextFieldParser(fileStream, Encoding.GetEncoding(config.EncodingID)))
      {
        parser.SetDelimiters(config.Delimiters.Select(x => new string(x, 1)).ToArray());
        parser.HasFieldsEnclosedInQuotes = config.HasFieldsEnclosedInQuotes;

        while (!parser.EndOfData)
        {
          try
          {
            fields = parser.ReadFields();
            string dateString = fields.ElementAtOrDefault(config.TimestampFieldIndex);

            // check for 'now playing'
            if (string.IsNullOrEmpty(dateString) && scrobbleMode == ScrobbleMode.Normal)
              continue;

            DateTime date = DateTime.Now;
            if (scrobbleMode == ScrobbleMode.Normal && !TryParseDateString(dateString, out date))
              throw new Exception("Timestamp could not be parsed!");

            // try to get optional parameters first
            string album = fields.ElementAtOrDefault(config.AlbumFieldIndex);
            string albumArtist = fields.ElementAtOrDefault(config.AlbumArtistFieldIndex);
            string timePlayedMS = fields.ElementAtOrDefault(config.MillisecondsPlayedFieldIndex);

            // filter short played songs
            if (config.FilterShortPlayedSongs &&
                TimeSpan.TryParse(timePlayedMS, out TimeSpan msPlayed) &&
                msPlayed.TotalMilliseconds <= config.MillisecondsPlayedThreshold)
              continue;
            else
            {
              var s = new Scrobble(fields[config.ArtistFieldIndex], album, fields[config.TrackFieldIndex], date.AddSeconds(1))
              {
                AlbumArtist = albumArtist
              };
              scrobbles.Add(s);
            }
          }
          catch (Exception ex)
          {
            string errorString = $"CSV line number: {parser.LineNumber - 1},";

            // fields is old in this case
            if (ex is not MalformedLineException)
            {
              foreach (string s in fields)
              {
                errorString += $"{s},";
              }
            }

            errorString += ex.Message;
            errors.Add(errorString);
          }
        }
      }

      return (scrobbles, errors);
    }

    /// <summary>
    /// Tries to parse a string to a DateTime.
    /// </summary>
    /// <param name="dateString">String to parse.</param>
    /// <param name="dateTime">Outgoing DateTime.</param>
    /// <returns>True if <paramref name="dateString"/> was successfully parsed,
    /// otherwise false.</returns>
    public static bool TryParseDateString(string dateString, out DateTime dateTime)
    {
      if (!DateTime.TryParse(dateString, out dateTime))
      {
        bool parsed;
        // try different formats until succeeded
        foreach (string format in _formats)
        {
          parsed = DateTime.TryParseExact(dateString, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);
          if (parsed)
            return true;
        }

        return false;
      }

      return true;
    }
  }
}