using IF.Lastfm.Core.Api;
using Microsoft.AspNetCore.Components;

namespace WebScrubbler
{
  public class LastfmClientService : IDisposable
  {
    #region Properties

    [Inject]
    protected HttpClient? HttpClient { get; private set; }

    private LastfmClient? _client;

    #endregion Properties

    public LastfmClientService()
    {
      LastAuthHelper.AuthDeleted += LastAuthHelper_AuthDeleted;
    }

    public async Task<LastfmClient?> GetClient() 
    {
      if (_client == null)
      {
        var auth = await LastAuthHelper.FromLocalStorage();
        if (auth != null)
          _client = new LastfmClient((LastAuth)auth, HttpClient);
      }

      return _client;
    }

    private void LastAuthHelper_AuthDeleted(object? sender, EventArgs e)
    {
      _client?.Dispose();
      _client = null;
    }

    public void Dispose()
    {
      LastAuthHelper.AuthDeleted -= LastAuthHelper_AuthDeleted;
    }
  }
}