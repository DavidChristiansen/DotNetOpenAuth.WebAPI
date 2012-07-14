using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Security.Principal;
using System.Web;
using Common.Logging;
using DotNetOpenAuth.OAuth2;

namespace DotNetOpenAuth.WebAPI {
    public class WebAPIResourceServer : ResourceServer {
        public WebAPIResourceServer(IAccessTokenAnalyzer accessTokenAnalyzer) : base(accessTokenAnalyzer) {
        }
        public IPrincipal GetPrincipal(HttpRequestMessage request, params string[] requiredScopes) {
            return base.GetPrincipal(new HttpRequestWrapper(HttpContext.Current.Request), requiredScopes);
        }
    }
}