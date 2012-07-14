using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetOpenAuth.WebAPI.HostSample.Infrastructure.OAuth;

namespace DotNetOpenAuth.WebAPI.HostSample {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static DatabaseKeyNonceStore KeyNonceStore { get; set; }

        /// <summary>
        /// Gets the transaction-protected database connection for the current request.
        /// </summary>
        public static DataClassesDataContext DataContext {
            get {
                DataClassesDataContext dataContext = DataContextSimple;
                if (dataContext == null) {
                    dataContext = new DataClassesDataContext();
                    dataContext.Connection.Open();
                    dataContext.Transaction = dataContext.Connection.BeginTransaction();
                    DataContextSimple = dataContext;
                }

                return dataContext;
            }
        }

        public static User LoggedInUser {
            get { return DataContext.Users.SingleOrDefault(user => user.OpenIDClaimedIdentifier == HttpContext.Current.User.Identity.Name); }
        }

        private static DataClassesDataContext DataContextSimple {
            get {
                if (HttpContext.Current != null) {
                    return HttpContext.Current.Items["DataContext"] as DataClassesDataContext;
                } else {
                    throw new InvalidOperationException();
                }
            }

            set {
                if (HttpContext.Current != null) {
                    HttpContext.Current.Items["DataContext"] = value;
                } else {
                    throw new InvalidOperationException();
                }
            }
        }
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            AuthenticationConfig.ConfigureGlobal(GlobalConfiguration.Configuration);
            KeyNonceStore = new DatabaseKeyNonceStore();
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }


        protected void Application_Error(object sender, EventArgs e) {
            // In the event of an unhandled exception, reverse any changes that were made to the database to avoid any partial database updates.
            var dataContext = DataContextSimple;
            if (dataContext != null) {
                dataContext.Transaction.Rollback();
                dataContext.Connection.Close();
                dataContext.Dispose();
                DataContextSimple = null;
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e) {
            CommitAndCloseDatabaseIfNecessary();
        }

        private static void CommitAndCloseDatabaseIfNecessary() {
            var dataContext = DataContextSimple;
            if (dataContext != null) {
                dataContext.SubmitChanges();
                dataContext.Transaction.Commit();
                dataContext.Connection.Close();
            }
        }
    }
}