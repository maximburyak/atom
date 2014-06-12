using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Atom.Main.Services;
using Atom.Main.Services.Filters;
using BeValued.Security;
using BeValued.Security.Web;
using NHibernate;

namespace Atom.Main.Controllers
{
    [Authorize]
	public class SecurityController : BaseController
	{
	    private readonly IFederationService _federationService;
	    private SecurityAdminService AdminService { get; set; }
		private AtomCacheManager CacheManager { get; set; }
		private const string ApplicationName = "Atom.Main";

		private string ConnStr { get; set; }

        public SecurityController(ISession session, IFederationService federationService)
		{
            _federationService = federationService;
			ConnStr = ConfigurationManager.ConnectionStrings[ApplicationName].ConnectionString;
			AdminService = new SecurityAdminService(ConnStr, ApplicationName);
			CacheManager = new AtomCacheManager(session);
		}

		[HttpGet]
		public ActionResult Index(string ReturnURL)
		{
            return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult Login(string ReturnURL)
		{
			if (User.Identity.IsAuthenticated && !String.IsNullOrEmpty(ReturnURL))
				Response.Redirect(ReturnURL);

			return View();
		}

		public ActionResult Logout()
		{
            _federationService.SignOut();
		    return null;
		}

		[HttpPost]
		public ActionResult Login(string username, string password, bool rememberMe, string ReturnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				if (!String.IsNullOrEmpty(ReturnUrl))
					Response.Redirect(ReturnUrl);

				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IT", Order = 2)]
		public ActionResult AddUserToApplication(string au_userID)
		{
			try
			{
				if (String.IsNullOrEmpty(au_userID))
					throw new ArgumentException(ApplicationResources.UserIDNotPresent);

				var granted = RoleManager.AddApplicationAccess(au_userID, User.Identity.FindValue(Claims.Username));

				RedirectToActionMessage(!granted, (granted ? ApplicationResources.UserAddSuccess : ApplicationResources.UserAddFail));
				return RedirectToAction("Admin");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("adduser", ex);
			}
			return View("Admin");
		}

		[HttpPost, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IT", Order = 2)]
		public ActionResult RemoveUserFromApplication(string ru_userID)
		{
			try
			{
				if (String.IsNullOrEmpty(ru_userID))
					throw new ArgumentException(ApplicationResources.UserIDNotPresent);

                var removed = RoleManager.RemoveApplicationAccess(ru_userID, User.Identity.FindValue(Claims.Username));

				RedirectToActionMessage(!removed, (removed ? ApplicationResources.UserRemovalSuccess : ApplicationResources.UserRemovalFail));
				return RedirectToAction("Admin");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("removeuser", ex);
			}
			return View("Admin");
		}

		[HttpPost, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IT", Order = 2)]
		public ActionResult AddUserToRole(string ar_userID, string ar_roleName)
		{
			try
			{
				if (String.IsNullOrEmpty(ar_userID) || String.IsNullOrEmpty(ar_roleName))
					throw new ArgumentException(ApplicationResources.RoleAndUserNotSupplied);

				RoleManager.AddUserToRoles(ar_userID, ar_roleName);
				RedirectToActionMessage(false, ApplicationResources.RoleAddSuccess);
				return RedirectToAction("Admin");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("addrole", ex);
			}
			return View("Admin");
		}

		[HttpPost, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IT", Order = 2)]
		public ActionResult RemoveUserFromRole(string rr_userID, string rr_roleName)
		{
			try
			{
				if (String.IsNullOrEmpty(rr_userID) || String.IsNullOrEmpty(rr_roleName))
					throw new ArgumentException(ApplicationResources.RoleAndUserNotSupplied);

				RoleManager.RemoveUserFromRoles(rr_userID, rr_roleName);
				RedirectToActionMessage(false, ApplicationResources.RoleRemovalSuccess);
				return RedirectToAction("Admin");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("removerole", ex);
			}
			return View("Admin");
		}

		[HttpGet, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IT", Order = 2)]
		public ActionResult Admin()
		{
			return View(AdminService.SecurityAdminViewModel());
		}

		[HttpGet, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IT", Order = 2)]
		public ActionResult RefreshCache()
		{
			CacheManager.RemoveCacheItems();
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public JsonResult RolesForUser(string userID)
		{
			var roles = AdminService.GetRolesForUser(userID);
			return Json(roles);
		}

		[HttpPost]
		public JsonResult RolesUserHas(string userID)
		{
			var roles = AdminService.GetExistingRoleForUser(userID);
			return Json(roles);
		}

        public ActionResult FusionAutoLogin(
            string authkey, 
            string authpw, 
            string authuser, 
            string workitemid, 
            string system, 
            string additionalinfo, 
            string systemid)
        {
            if (CheckAutoLoginDetails(authkey, authpw, authuser, system))
            {
                var search = (string.IsNullOrEmpty(workitemid) ? "" : "item:" + workitemid);
                var controller = string.IsNullOrEmpty(additionalinfo) ? "Search" : "Incident";
                var action = string.IsNullOrEmpty(additionalinfo) ? "Index" : "Add";

                return new RedirectToRouteResult("Fusion_Search", new RouteValueDictionary(new { controller, action, id = "", search, additionalinfo, systemid }));
            }

            RedirectToActionMessage(true, "Could Not Authenticate automatically");
            return RedirectToAction("Login", "Security");
        }

		private static bool CheckAutoLoginDetails(string authkey, string authpw, string authuser, string system)
		{
			if (string.IsNullOrEmpty(authkey) || string.IsNullOrEmpty(authpw) || string.IsNullOrEmpty(authuser) || string.IsNullOrEmpty(system)) return false;

			return authkey.Equals(AtomResourceHelper.ReadResourceValue(typeof(ApplicationResources), string.Format("FusionAuthKey_{0}", system))) && 
                authpw.Equals(AtomResourceHelper.ReadResourceValue(typeof(ApplicationResources), string.Format("FusionAuthPW_{0}", system))) && 
                authuser.Equals(AtomResourceHelper.ReadResourceValue(typeof(ApplicationResources), string.Format("FusionAuthUser_{0}", system)));
		}
	}
}


