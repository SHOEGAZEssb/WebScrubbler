using BlazorStrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using WebScrubbler;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
LocalStorageHelper.Initialize(builder.Services.Where(i => i.ServiceType == typeof(IJSRuntime)).FirstOrDefault()?.ImplementationInstance as IJSRuntime);
builder.Services.AddScoped(s => new LastfmClientService());

builder.Services.AddBlazorStrap();

await builder.Build().RunAsync();