using System.Runtime.Serialization;
using System.Text;

namespace WebScrubbler.Parser
{
  [DataContract]
  public class CSVParserConfiguration
  {
    #region Properties

    [DataMember]
    public int TrackFieldIndex;

    [DataMember]
    public int ArtistFieldIndex;

    [DataMember]
    public int AlbumFieldIndex;

    [DataMember]
    public int AlbumArtistFieldIndex;

    [DataMember]
    public int TimestampFieldIndex;

    [DataMember]
    public int MillisecondsPlayedFieldIndex;

    [DataMember]
    public int MillisecondsPlayedThreshold;

    [DataMember]
    public bool FilterShortPlayedSongs;

    [DataMember]
    public string Delimiters;

    [DataMember]
    public bool HasFieldsEnclosedInQuotes;

    [DataMember]
    public int EncodingID;

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor that creates a default configuration.
    /// </summary>
    public CSVParserConfiguration() 
    { 
      TrackFieldIndex = 0;
      ArtistFieldIndex = 1;
      AlbumFieldIndex = 2;
      AlbumArtistFieldIndex = 3;
      TimestampFieldIndex = 4;
      MillisecondsPlayedFieldIndex = 5;
      MillisecondsPlayedThreshold = 1000;
      FilterShortPlayedSongs = true;
      Delimiters = ",";
      HasFieldsEnclosedInQuotes = true;
      EncodingID = Encoding.UTF8.CodePage;
    }

    #endregion Construction
  }
}