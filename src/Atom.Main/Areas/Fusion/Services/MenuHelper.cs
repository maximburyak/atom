using System.Collections;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Atom.Main.Areas.Fusion.Services.Domain;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class MenuHelper
	{
		public static string Menu(this HtmlHelper helper)
		{
			var currentNode = SiteMap.CurrentNode;
			if (currentNode == null)
				currentNode = SiteMap.RootNode;

			var sb = new StringBuilder();

			// Create opening unordered list tag
			sb.Append("<ul>");

			// Render each top level node
			var topLevelNodes = SiteMap.RootNode.ChildNodes;
			foreach (SiteMapNode node in topLevelNodes)
			{
				sb.AppendLine("<li>");
				if (currentNode == node || currentNode.ParentNode == node)
					sb.AppendFormat("<a title=\"{0}\" class=\"selectedmenuitem\" href=\"{1}\"><span>{2}</span></a>", node.Description, node.Url, helper.Encode(node.Title));
				else
					sb.AppendFormat("<a title=\"{0}\" href=\"{1}\"><span>{2}</span></a>", node.Description, node.Url, helper.Encode(node.Title));

				sb.AppendLine("</li>");
			}
			sb.Append(helper.LogInOutLink(HttpContext.Current.Request.IsAuthenticated));

			// Close unordered list tag
			sb.Append("</ul>");

			return sb.ToString();
		}

		public static string SubMenu(this HtmlHelper helper)
		{
			var currentNode = SiteMap.CurrentNode;
			if (currentNode == null)
				return "";

			// Render each sub level node
			// Need to check if we are clicking a sub-menu element as well as a top level element!!
			var subMenuNodes = (currentNode.HasChildNodes ? currentNode.ChildNodes
				// If current node doesnt have any children, does its parent?	
				: ((currentNode.ParentNode.HasChildNodes && currentNode.ParentNode != SiteMap.RootNode) ? currentNode.ParentNode.ChildNodes : null));

			if (subMenuNodes != null)
			{
				var sb = new StringBuilder();
				// Create opening unordered list tag
				sb.Append("<ul>");

				foreach (SiteMapNode node in subMenuNodes)
				{
					var list = (string[])((ArrayList) node.Roles).ToArray(typeof(string));
					if (!RoleAuthorizationService.ViewSubMenuOptions(list) && list.Length>0) continue;
					sb.AppendLine("<li>");
					
					if (SiteMap.CurrentNode == node)
						sb.AppendFormat("<a title=\"{0}\" class=\"selectedmenuitem\" href=\"{1}\"><span>{2}</span></a>", node.Description, node.Url, helper.Encode(node.Title));
					else
						sb.AppendFormat("<a title=\"{0}\" href=\"{1}\"><span>{2}</span></a>", node.Description, node.Url, helper.Encode(node.Title));

					sb.AppendLine("</li>");
				}

				// Close unordered list tag
				sb.Append("</ul>");
				return sb.ToString();
			}
			return "";
		}

	}
}
