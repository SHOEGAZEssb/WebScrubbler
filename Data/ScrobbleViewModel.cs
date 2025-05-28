using IF.Lastfm.Core.Objects;
using Microsoft.AspNetCore.Components;
using System;

namespace WebScrubbler.Data
{
  public class ScrobbleViewModel(Scrobble scrobble)
  {
    #region Properties

    public string Track => _scrobble.Track;
    public string Artist => _scrobble.Artist;
    public string Album => _scrobble.Album;
    public string AlbumArtist => _scrobble.AlbumArtist;
    public DateTime Timestamp => _scrobble.TimePlayed.DateTime;

    public bool CanScrobble => Timestamp > DateTime.Now.Subtract(TimeSpan.FromDays(14));

    public bool ToScrobble
    {
      get => _toScrobble;
      set => _toScrobble = value;
    }
    private bool _toScrobble;

    #endregion Properties

    #region Member

    private readonly Scrobble _scrobble = scrobble;

    #endregion Member
  }
}
