using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.JSInterop;
using Newtonsoft.Json;

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

    public const string CSVPARSERCONFIGURATIONIDENTIFIER = "csvParserConfiguration";
    public const string JSONPARSERCONFIGURATIONIDENTIFIER = "jsonParserConfiguration";

    public static async Task<T> GetObject<T>(string localStorageIdentifier) where T : new()
    {
      if (_jsRuntime == null)
        throw new InvalidOperationException("No JSRuntime available");

      var serializedState = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", localStorageIdentifier);
      if (string.IsNullOrEmpty(serializedState))
        return new T();
      else
      {
        try
        {
          return JsonConvert.DeserializeObject<T>(serializedState) ?? throw new Exception($"Could not deserialize object of type {nameof(T)}");
        }
        catch
        {
          await DeleteObject(localStorageIdentifier);
          return new T();
        }
      }
    }

    public static async Task SaveToLocalStorage(object obj, string localStorageIdentifier)
    {
      if (_jsRuntime != null)
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", localStorageIdentifier, JsonConvert.SerializeObject(obj));
    }

    public static async Task DeleteObject(string localStorageIdetifier)
    {
      if (_jsRuntime == null)
        throw new InvalidOperationException("No JSRuntime available");

      await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", localStorageIdetifier);
    }
  }
}