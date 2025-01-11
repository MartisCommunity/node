using Microsoft.AspNetCore.Authentication;

namespace Martiscoin.Features.NodeHost.Authentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";

        public string Scheme => DefaultScheme;

        public string AuthenticationType = DefaultScheme;
    }
}
