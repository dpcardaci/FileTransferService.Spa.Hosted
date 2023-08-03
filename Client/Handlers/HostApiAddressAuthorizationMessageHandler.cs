using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FileTransferService.Spa.Hosted.Client.Handlers
{
    public class HostApiAddressAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public HostApiAddressAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager, IConfiguration configuration)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { navigationManager.BaseUri },
                scopes: configuration.GetSection("FileTransferServiceHost:Scopes")
                            .Get<List<string>>()
            );
        }
    }
}
