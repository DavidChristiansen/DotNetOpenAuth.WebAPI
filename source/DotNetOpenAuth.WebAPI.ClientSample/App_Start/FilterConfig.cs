using System.Web;
using System.Web.Mvc;

namespace DotNetOpenAuth.WebAPI.ClientSample {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
