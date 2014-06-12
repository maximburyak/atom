using System.Configuration;
using System.Web.Mvc;
using BeValued.Mvc.Filters;

namespace Atom.Main.Filters
{
	public class RedirectToSsl : RequiresSSL, IAuthorizationFilter
	{
		public override string Host { get { return ConfigurationManager.AppSettings["url"]; } set { } }
	}
}