using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetOpenAuth.WebAPI {
    public class CorsHandler : DelegatingHandler {
        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            bool isCorsRequest = request.Headers.Contains(Origin);
            bool isPreflightRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest) {
                if (isPreflightRequest) {
                    return Task.Factory.StartNew<HttpResponseMessage>(() => {
                                                                          HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                                                                          response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

                                                                          string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
                                                                          if (accessControlRequestMethod != null) {
                                                                              response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                                                                          }

                                                                          string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
                                                                          if (!string.IsNullOrEmpty(requestedHeaders)) {
                                                                              response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                                                                          }

                                                                          return response;
                                                                      }, cancellationToken);
                } else {
                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t => {
                                                                                                            HttpResponseMessage resp = t.Result;
                /*This fixes the problem with CorsRequests using credentials in FF and Chrome*/                                                                                            
                resp.Headers.Add("Access-Control-Allow-Credentials", "true");
                resp.Headers.Add(AccessControlAllowMethods, "GET, POST, PUT, DELETE");
                resp.Headers.Add(AccessControlAllowHeaders, "Authorization");                                                                                                            
                                                                                                            
                                                                                                            resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                                                                                                            return resp;
                                                                                                        });
                }
            } else {
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
