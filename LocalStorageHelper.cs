using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebScrubbler.Parser;

namespace WebScrubbler
{
  public static class LocalStorageHelper
  {
    private static IJSRuntime? _jsRuntime;

    public static void Initialize(IJSRuntime? runtime) 
    { 
      _jsRuntime = runtime ?? throw new ArgumentNullException(nameof(runtime));
    }

    #region ILastAuth

    public static event EventHandler? AuthDeleted;

    private const string LASTAUTHIDENTIFIER = "lastAuth";

    public static async Task<ILastAuth?> GetILastAuth()
    {
      if (_jsRuntime != null)
      {
        var serializedState = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", LASTAUTHIDENTIFIER);
        if (!string.IsNullOrEmpty(serializedState))
        {
          // Rehydrate state from browser storage
          var user = JsonConvert.DeserializeObject<LastUserSession>(serializedState);
          var auth = new LastAuth("69fbfa5fdc2cc1a158ec3bffab4be7a7", "30a6ed8a75dad2aa6758fa607c53adb5");
          auth.LoadSession(user);
          return auth;
        }
      }

      return null;
    }

    public static async Task SaveToLocalStorage(this ILastAuth auth)
    { 
      if (_jsRuntime != null)
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", LASTAUTHIDENTIFIER, JsonConvert.SerializeObject(auth.UserSession));
    }

    public static async Task DeleteILastAuth()
    {
      if (_jsRuntime != null)
      {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", LASTAUTHIDENTIFIER);
        AuthDeleted?.Invoke(null, EventArgs.Empty);
      }
    }

    #endregion ILastAuth

    #region CSVParserConfiguration

    private const string CSVPARSERCONFIGURATIONIDENTIFIER = "csvParserConfiguration";

    public static async Task<CSVParserConfiguration> GetCSVParserConfiguration()
    {
      if (_jsRuntime == null)
        throw new InvalidOperationException("No JSRuntime available");
      else
      {
        var serializedState = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", CSVPARSERCONFIGURATIONIDENTIFIER);
        if (string.IsNullOrEmpty(serializedState))
          return new CSVParserConfiguration();
        else
        {
          try
          {
            return JsonConvert.DeserializeObject<CSVParserConfiguration>(serializedState);
          }
          catch 
          {
            await DeleteCSVParserConfiguration();
            return new CSVParserConfiguration();
          }
        }
      }
    }

    public static async Task SaveToLocalStorage(this CSVParserConfiguration csvParserConfiguration)
    {
      if (_jsRuntime != null)
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", CSVPARSERCONFIGURATIONIDENTIFIER, JsonConvert.SerializeObject(csvParserConfiguration));
    }

    public static async Task DeleteCSVParserConfiguration()
    {
      if (_jsRuntime == null)
        throw new InvalidOperationException("No JSRuntime available");
      else
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", CSVPARSERCONFIGURATIONIDENTIFIER);
    }

    #endregion CSVParserConfiguration
  }
}