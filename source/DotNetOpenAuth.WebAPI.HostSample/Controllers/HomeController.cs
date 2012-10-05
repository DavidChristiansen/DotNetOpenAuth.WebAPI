using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.WebAPI.HostSample.Infrastructure.OAuth;

namespace DotNetOpenAuth.WebAPI.HostSample.Controllers {
    public class HomeController : Controller {
        string ConnectionString {
            get {
                string databasePath = Path.Combine(Server.MapPath(Request.ApplicationPath), "App_Data");
                if (!Directory.Exists(databasePath)) {
                    Directory.CreateDirectory(databasePath);
                }
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString.Replace("|DataDirectory|", databasePath);
                return connectionString;
            }
        }
        public ActionResult Index() {
            var dc = new DataClassesDataContext(ConnectionString);
            ViewBag.DBExists = dc.DatabaseExists();
            return View();
        }

        public ActionResult CreateDatabase() {
            var dc = new DataClassesDataContext(ConnectionString);
            if (dc.DatabaseExists()) {
                dc.DeleteDatabase();
            }
            try {
                dc.CreateDatabase();

                // Add the necessary row for the sample client.
                dc.Clients.InsertOnSubmit(new Client {
                    ClientIdentifier = "samplewebapiconsumer",
                    ClientSecret = "samplesecret",
                    Name = "Some sample client",
                });
                dc.Clients.InsertOnSubmit(new Client {
                    ClientIdentifier = "sampleImplicitConsumer",
                    Name = "Some sample client used for implicit grants (no secret)",
                    Callback = "http://localhost:59722/",
                });

                dc.SubmitChanges();

                // Force the user to log out because a new database warrants a new row in the users table, which we create
                // when the user logs in.
                FormsAuthentication.SignOut();
                ViewData["Success"] = true;
            }
            catch (SqlException ex) {
                ViewData["Error"] = string.Join("<br>", ex.Errors.OfType<SqlError>().Select(er => er.Message).ToArray());
            }

            return this.View("Database");
        }
    }
}