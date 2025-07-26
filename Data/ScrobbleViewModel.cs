using IF.Lastfm.Core.Objects;

namespace WebScrubbler.Data
{
  public class ScrobbleViewModel(Scrobble scrobble)
  {
    #region Properties

    public Scrobble Scrobble { get; } = scrobble;

    public string Track => Scrobble.Track;
    public string Artist => Scrobble.Artist;
    public string Album => Scrobble.Album;
    public string AlbumArtist => Scrobble.AlbumArtist;
    public DateTime Timestamp => Scrobble.TimePlayed.DateTime;

    public bool CanScrobble => Timestamp > DateTime.Now.Subtract(TimeSpan.FromDays(14));

    public bool ToScrobble
    {
      get => _toScrobble;
      set => _toScrobble = value;
    }
    private bool _toScrobble;

    #endregion Properties
  }
}