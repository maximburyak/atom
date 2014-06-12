using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class HtmlHelpers
	{
		private const string _missingImage = "missing.gif";

		public static string LogInOutLink(this HtmlHelper helper, bool isLoggedIn)
		{
			var tb = new TagBuilder("a") { InnerHtml = "<span>{0}</span>".With((isLoggedIn ? "Logout" : "Log in")) };
			tb.Attributes.Add("href",
					"/Security/{0}?{1}".With(
						(isLoggedIn ? "LogOut" : "LogIn"),
				// Add in Default redirect
						("ReturnUrl=" + HttpContext.Current.Request.RawUrl ?? "")
					));
			tb.Attributes.Add("title", "{0}".With((isLoggedIn ? "Log out" : "Log in")));
			return "<li>" + tb + "</li>";
		}

		public static string Avatar(this HtmlHelper helper, User user, object imageHtmlAttributes)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			if (user == null)
				return helper.ImageTag(urlHelper.Avatar(_missingImage), "Avatar not set", "Avatar missing", imageHtmlAttributes);
			if (user.Profile == null || user.Profile.CurrentAvatar == 0)
				return helper.ImageTag(urlHelper.Avatar(_missingImage), "Avatar not set", "Avatar missing", imageHtmlAttributes);

			var fileExtension = user.Profile.Avatars.Any(x => !string.IsNullOrEmpty(x.FileExtension) && x.Id == user.Profile.CurrentAvatar) ? user.Profile.Avatars.First(x => x.Id == user.Profile.CurrentAvatar && !string.IsNullOrEmpty(x.FileExtension)).FileExtension : "gif";
			return helper.ImageTag(urlHelper.Avatar(String.Format("{0}_{1}.{2}", user.UserID, user.Profile.CurrentAvatar, fileExtension)), user.Name, user.Name, imageHtmlAttributes);
		}

		public static string AvatarForWorkItem(this HtmlHelper helper, User user, object imageHtmlAttributes, WorkItem item)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var haveDept = item.Department.Id >= 0;
			var assigned = item.AssignedTo != null;
			var altText = "Missing Avatar" + (assigned ? " for " + item.AssignedTo.Name : "");
			if (!assigned && haveDept)
				return helper.ImageTag(urlHelper.Avatar("dept-" + item.Department.Description + ".gif"), item.Department.Description,
									   item.Department.Description, imageHtmlAttributes);
			
			if (user == null || user.Profile == null || user.Profile.CurrentAvatar == 0)
				return helper.ImageTag(urlHelper.Avatar(_missingImage), altText, altText, imageHtmlAttributes);

			return Avatar(helper, user, imageHtmlAttributes);
		}
		public static string ErrorBoxDisplay(this HtmlHelper html, bool validModelState)
		{
			return (validModelState) ? "display:none" : "";
		}

		public static string AvatarList(this HtmlHelper helper, User user)
		{
			if (user.Profile == null || user.Profile.Avatars == null)
				return "You have not yet uploaded an avatar image.";

			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var ul = new TagBuilder("ul");
			var li = new TagBuilder("li");
			foreach (var a in user.Profile.Avatars)
			{
				if (a.Id == user.Profile.CurrentAvatar) continue;
				var fileExtension = string.IsNullOrEmpty(a.FileExtension) ? "gif" : a.FileExtension;
				li.InnerHtml = helper.ImageTag(urlHelper.Avatar("{0}_{1}.{2}".With(user.UserID, a.Id, fileExtension)), user.Name, user.Name, new { @class = "avatar" });
				li.InnerHtml += helper.RadioButton("CurrentAvatar", a.Id, false);
				ul.InnerHtml += li.ToString();
			}
			return ul.ToString();
		}

		public static string ImageLink(this HtmlHelper helper, string actionName, string imageUrl, string alternateText, object routeValues, object linkHtmlAttributes, object imageHtmlAttributes)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var url = urlHelper.Action(actionName, routeValues);

			// Create link  
			var linkTagBuilder = new TagBuilder("a");
			linkTagBuilder.MergeAttribute("href", url);
			linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

			// Create image  
			var imageTagBuilder = new TagBuilder("img");
			imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
			imageTagBuilder.MergeAttribute("alt", urlHelper.Encode(alternateText));
			imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

			// Add image to link  
			linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

			return linkTagBuilder.ToString();
		}

		public static string ImageTag(this HtmlHelper helper, string imageUrl, string alternateText, string title, object imageHtmlAttributes)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var imageTagBuilder = new TagBuilder("img");
			imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
			imageTagBuilder.MergeAttribute("alt", alternateText);
			imageTagBuilder.MergeAttribute("title", title);
			imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));
			return imageTagBuilder.ToString(TagRenderMode.SelfClosing);
		}

		public static string ProfileFilterButton(this HtmlHelper helper, User user)
		{

			var tb = new TagBuilder("button") { InnerHtml = "{0}".With(user.Profile.ShowFilters ? "Hide Filters" : "Show Filters") };
			tb.Attributes.Add("onclick", "document.location.replace('/Fusion/Profile/UpdateFilter');");
			return tb.ToString();
		}

		public static string FilterDeleteImage(this HtmlHelper helper)
		{
			return ImageLink(helper, "", "/Areas/Fusion/Content/Images/icons/delete.png", "remove filter", null, new { id = "filterdelete", href = "#", title = "Delete Filter" }, new { @class = "filter-delete" });
		}

		public static string FilterDefaultImage(this HtmlHelper helper)
		{
			return ImageLink(helper, "", "/Areas/Fusion/Content/Images/icons/star.png", "make default", null, new { id = "defaultfilter", href = "#", title = "Make Filter Default" }, new { @class = "filter-default" });
		}

		public static string EditItemImage(this HtmlHelper helper, string item)
		{
			return ImageLink(helper, null, "/Areas/Fusion/Content/Images/icons/edit_item.png", "edit item", null, new { id = "edititem_" + item, href = "#", title = "Edit Item " + item }, new { @class = "edit-item"});
		}

		public static string EditItemSaveImage(this HtmlHelper helper, string item)
		{
			return ImageLink(helper, "", "/Areas/Fusion/Content/Images/icons/edit_save.png", "save edited item", null, new { id = "save_edititem_" + item, href = "#", title = "Save Edited Item " + item }, new { @class = "edit-item" });
		}

		public static string FilterDefaultDeleteImage(this HtmlHelper helper)
		{
			return ImageLink(helper, "", "/Areas/Fusion/Content/Images/icons/star_delete.png", "", null, new { id = "defaultfilterdelete", href = "#", style = "display:none", title = "Remove Default Filter" }, new { @class = "filter-delete" });
		}

		public static void RenderPartialAction<TController>(this HtmlHelper helper, Func<TController, PartialViewResult> actionToRender)
			where TController : Controller, new()
		{
			var arg = new TController { ControllerContext = helper.ViewContext.Controller.ControllerContext };
			actionToRender(arg).ExecuteResult(arg.ControllerContext);
		}

	}
}
