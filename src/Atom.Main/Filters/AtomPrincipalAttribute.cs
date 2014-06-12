using System;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using StructureMap;

namespace Atom.Main.Filters
{
	public class AtomPrincipalAttribute : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var session = ObjectFactory.GetInstance<ISession>();
			var user = filterContext.HttpContext.User;

			if (user.Identity.Name == null) return;

			//var rolePrincipal = new RolePrincipal(user.Identity);
			var atomIdentity = new AtomIdentity(user.Identity.Name);
			var atomUser = session.QueryOver<User>().Where(x => x.UserID == user.Identity.Name).Cacheable().SingleOrDefault();
			var principal = new AtomPrincipal(atomIdentity, RoleManager.GetRolesForUser(user.Identity.Name)) { Date = DateTime.Now, User = atomUser };
			System.Threading.Thread.CurrentPrincipal = filterContext.HttpContext.User = principal;
		}
	}
}