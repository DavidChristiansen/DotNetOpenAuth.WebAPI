using System.Globalization;
using System.Net;
using DotNetOpenAuth.OAuth2;

namespace DotNetOpenAuth.WebAPI {
    public static class UserAgentClientExtensions {
        public static void AuthorizeRequest(this UserAgentClient client, WebClient webClient, IAuthorizationState authorization) {
            webClient.Headers[HttpRequestHeader.Authorization] = string.Format(CultureInfo.InvariantCulture, "Bearer {0}", authorization.AccessToken);
        }
    }
}