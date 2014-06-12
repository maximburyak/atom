using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Atom.Areas.Fusion.Data;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class LinkHelper
	{
		public static string LinkedItems(this HtmlHelper helper, IEnumerable<WorkItemLinkDto> workitemlinks)
		{
			var sb = new StringBuilder();
			sb.Append("<div style=\"width:200px\">");
			foreach (var wil in workitemlinks)
			{
				sb.Append("<div class=\"caseitem\" style=\"height:25px\"> <span style=\"float:left\" class=\"case-linked-list\">{0}</span><span style=\"float:right\"> {1}</span></div>".With(LinkedItemText(helper, wil),
																			 LinkedItemCheckBox(helper, wil)));
			}
			sb.Append("</div>");
			return workitemlinks.Any() ? sb.ToString() : string.Empty;
		}

		private static string LinkedItemText(this HtmlHelper helper, WorkItemLinkDto wil)
		{
			var tb = new TagBuilder("a") { InnerHtml = "<span>{0} ({1}) </span>".With(wil.RelatesToWorkItemId, wil.RelatesToType) };
			tb.Attributes.Add("href", "/Fusion/{0}/Details/{1}".With(wil.RelatesToType, wil.RelatesToWorkItemId));
			tb.Attributes.Add("target", "_blank");
			return tb.ToString();
		}

		private static string LinkedItemCheckBox(this HtmlHelper helper, WorkItemLinkDto wil)
		{
			var tb = new TagBuilder("input");
			tb.Attributes.Add("name", "selectedLinkedWorkItems");
			tb.Attributes.Add("id", wil.Id.ToString());
			tb.Attributes.Add("value", wil.Id.ToString());
			tb.Attributes.Add("type", "checkbox");
			tb.Attributes.Add("class", "chk");
			tb.Attributes.Add("style", "width:30px;");
			return tb.ToString();
		}

	}
}
