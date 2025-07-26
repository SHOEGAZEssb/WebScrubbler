using System.Runtime.Serialization;

namespace WebScrubbler.Parser
{
  [DataContract]
  public class JSONParserConfiguration
  {
    #region Properties

    public string TrackNameProperty;

    public string ArtistNameProperty;

    public string AlbumNameProperty;

    public string AlbumArtistNameProperty;

    public string TimestampProperty;

    public string MillisecondsPlayedProperty;

    [DataMember]
    public int MillisecondsPlayedThreshold;

    [DataMember]
    public bool FilterShortPlayedSongs;

    #endregion Properties

    #region Construction

    public JSONParserConfiguration()
    {
      TrackNameProperty = "master_metadata_track_name";
      ArtistNameProperty = "master_metadata_album_artist_name";
      AlbumNameProperty = "master_metadata_album_album_name";
      AlbumArtistNameProperty = "master_metadata_album_artist_name";
      TimestampProperty = "ts";
      MillisecondsPlayedProperty = "ms_played";
      MillisecondsPlayedThreshold = 1000;
      FilterShortPlayedSongs = true;
    }

    #endregion Construction
  }
}
