using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FileTransferService.Spa.Hosted.Client;
using FileTransferService.Spa.Hosted.Client.Handlers;
using FileTransferService.Spa.Hosted.Client.Extensions;
using FileTransferService.Spa.Hosted.Client.Factories;
using FileTransferService.Spa.Hosted.Client.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add authorization handler for calls to the Host API
builder.Services.AddScoped<HostApiAddressAuthorizationMessageHandler>();

// Add named HTTP client for calls to Host API
builder.Services.AddHttpClient("FileTransferService.Spa.Hosted.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<HostApiAddressAuthorizationMessageHandler>();

// Add named HTTP client for all other calls
builder.Services.AddHttpClient("DefaultHttpClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Set default HTTP client
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DefaultHttpClient"));

// Configure and add GraphServiceClient
var baseUrl = builder.Configuration.GetSection("MicrosoftGraph")["BaseUrl"];
var scopes = builder.Configuration.GetSection("MicrosoftGraph:Scopes")
    .Get<List<string>>();

builder.Services.AddGraphClient(baseUrl, scopes);

builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomAccountFactory>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("GroupMembership", policy => policy.RequireClaim("directoryGroup", "582bfd92-e8bd-4b03-a173-743d269b7a34"));
});

await builder.Build().RunAsync();
