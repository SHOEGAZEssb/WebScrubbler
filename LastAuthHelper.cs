using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace WebScrubbler
{
  public static class LastAuthHelper
  {
    public static event EventHandler? AuthDeleted;

    private static IJSRuntime _jsRuntime;

    public static void Initialize(IJSRuntime? runtime) 
    { 
      _jsRuntime = runtime ?? throw new ArgumentNullException(nameof(runtime));
    }

    public static async Task<ILastAuth?> FromLocalStorage()
    {
      var serializedState = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "lastAuth");
      if (!string.IsNullOrEmpty(serializedState))
      {
        // Rehydrate state from browser storage
        var user = JsonConvert.DeserializeObject<LastUserSession>(serializedState);
        var auth = new LastAuth("69fbfa5fdc2cc1a158ec3bffab4be7a7", "30a6ed8a75dad2aa6758fa607c53adb5");
        auth.LoadSession(user);
        return auth;
      }

      return null;
    }

    public static async Task SaveToLocalStorage(this ILastAuth auth)
    {      
      await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "lastAuth", JsonConvert.SerializeObject(auth.UserSession));
    }

    public static async Task DeleteAuthFromLocalStorage()
    {
      await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "lastAuth");
      AuthDeleted?.Invoke(null, EventArgs.Empty);
    }
  }
}
