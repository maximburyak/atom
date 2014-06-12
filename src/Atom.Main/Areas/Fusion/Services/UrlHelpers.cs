using System;
using System.Web.Mvc;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class UrlHelpers
	{
		public static string Image(this UrlHelper helper, string fileName)
		{
			return helper.Content(String.Format("/Areas/Fusion/Content/Images/{0}", fileName));
		}
		public static string Avatar(this UrlHelper helper, string filename)
		{
			return helper.Content(String.Format("/Areas/Fusion/Content/Images/avatars/{0}", filename));
		}
		public static string Signature(this UrlHelper helper, string filename)
		{
			return helper.Content(String.Format("/Areas/Fusion/Content/Images/signatures/{0}", filename));
		}
		public static string Stylesheet(this UrlHelper helper, string fileName)
		{
			return helper.Content(String.Format("/Areas/Fusion/Content/Css/{0}", fileName));
		}
		public static string Javascript(this UrlHelper helper, string fileName)
		{
			return helper.Content(String.Format("/content/scripts/{0}", fileName));
		}
		// Scripts specific to Fusion
		public static string JavascriptFusion(this UrlHelper helper, string fileName)
		{
			return helper.Content(String.Format("/Areas/Fusion/Content/Scripts/{0}", fileName));
		}
	}
}