using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Web;
using DotNetOpenAuth.WebAPI.HostSample.Infrastructure.OAuth;
using DotNetOpenAuth.WebAPI.HostSample.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DotNetOpenAuth.WebAPI.HostSample.Controllers {
    [HandleError]
    public class AccountController : Controller {

        public IFormsAuthentication FormsAuth { get; private set; }

        public IMembershipService MembershipService { get; private set; }
        public AccountController()
            : this(null, null) {
        }

        public AccountController(IFormsAuthentication formsAuth, IMembershipService service) {
            this.FormsAuth = formsAuth ?? new FormsAuthenticationService();
            this.MembershipService = service ?? new AccountMembershipService();
        }
        public ActionResult LogOn(string returnURL) {
            ViewBag.ReturnURL = returnURL;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, bool? rememberMe, string returnUrl) {
            this.FormsAuth.SignIn(userName, rememberMe ?? false);
            if (MvcApplication.DataContext.Users.FirstOrDefault(u => u.OpenIDClaimedIdentifier == userName) == null) {
                MvcApplication.DataContext.Users.InsertOnSubmit(new User {
                    OpenIDFriendlyIdentifier = userName,
                    OpenIDClaimedIdentifier = userName,
                });
            }

            if (!String.IsNullOrEmpty(returnUrl)) {
                return Redirect(returnUrl);
            } else {
                return RedirectToAction("Index", "Home");
            }
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************
        public ActionResult LogOff() {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }


    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication {
        void SignIn(string userName, bool createPersistentCookie);

        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication {
        public string SignedInUsername {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public DateTime? SignedInTimestampUtc {
            get {
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null) {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    return ticket.IssueDate.ToUniversalTime();
                } else {
                    return null;
                }
            }
        }

        public void SignIn(string userName, bool createPersistentCookie) {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService {
        MembershipCreateStatus CreateUser(string claimedIdentifier, string email);
    }

    public class AccountMembershipService : IMembershipService {
        private MembershipProvider provider;

        public AccountMembershipService()
            : this(null) {
        }

        public AccountMembershipService(MembershipProvider provider) {
            this.provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength {
            get {
                return this.provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password) {
            return this.provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email) {
            MembershipCreateStatus status;
            this.provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword) {
            MembershipUser currentUser = this.provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }

        public MembershipCreateStatus CreateUser(string claimedIdentifier, string email) {
            MembershipCreateStatus status;
            this.provider.CreateUser(claimedIdentifier, claimedIdentifier, email, null, null, true, null, out status);
            return status;
        }
    }
}
