using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atom.Main.Areas.Fusion.Services.Filters
{
    public class AuthorizeRole : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.ActionDescriptor.ActionName == "NotInRole")
                return;

            if (RoleManager.IsUserInRole(Roles))
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            else
            {
                // auth failed, redirect to login page
                filterContext.Result = new RedirectToRouteResult("Fusion_Search", new RouteValueDictionary(new { controller = "Search", action = "NotInRole", id = "" }));
            }
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

    }
}
