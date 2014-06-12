using System;
using System.Text;
using System.Web.Mvc;

namespace Atom.Main.Areas.Dashboard.Services
{
	public static class HtmlHelpers
	{
		public static string Document(this HtmlHelper helper, string id, string title, string severity, string type, string draggable)
		{
			var sb = new StringBuilder("");
			var doctype = (severity == "0") ? "project" : "crf";

			//sb.Append(String.Format("<span class=\"{0}crf severity{1}\" title=\"{2}\" data=\"{3}\">", draggable, severity, title, doctype));
			//sb.Append(String.Format("<b class=\"crfno\" data=\"{0}\">{1}</b>", doctype, id));
			//sb.Append(String.Format("<br /><b class=\"info\" id=\"{0}\" data=\"{1}\" title=\"View Details\"></b>", id, doctype));
			//sb.Append(String.Format("<b class=\"crf-icons\">{0}</b>", type));
			//sb.Append("</span>");
			//return sb.ToString().Trim();

			sb.Append(String.Format("<span class=\"{0}crf severity{1}\" title=\"{2}\" data=\"{3}\">", draggable, severity, title, doctype));
			sb.Append(String.Format("<p><b class=\"crf-icons\">{0}</b>", type));
			sb.Append(String.Format("<b class=\"info\" id=\"{0}\" data=\"{1}\" title=\"View Details\">&nbsp;&nbsp;&nbsp;&nbsp;</b>", id, doctype));
			sb.Append(String.Format("<br /><b class=\"crfno\" data=\"{0}\">{1}</b>", doctype, id));
			sb.Append("</p></span>");
			return sb.ToString().Trim();
		}
	}
}