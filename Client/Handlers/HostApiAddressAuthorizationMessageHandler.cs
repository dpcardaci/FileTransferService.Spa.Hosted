using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FileTransferService.Spa.Hosted.Client.Handlers
{
    public class HostApiAddressAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public HostApiAddressAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { navigationManager.BaseUri },
                scopes: new[] { "api://60a0d398-e5ab-43c6-a01f-f27dd8f6ab59/API.Access" }
            );
        }
    }
}
