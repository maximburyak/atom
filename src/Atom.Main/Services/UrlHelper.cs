using System;
using System.Web.Mvc;

namespace Atom.Main.Services
{
	public static class UrlHelpers
	{
		public static string Javascript(this UrlHelper helper, string fileName)
		{
			return helper.Content(String.Format("/Content/Scripts/{0}", fileName));
		}
	}
}
