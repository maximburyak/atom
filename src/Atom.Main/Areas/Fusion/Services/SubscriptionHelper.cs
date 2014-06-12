using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class SubscriptionHelper
	{
		public static string SubscriptionLink(this HtmlHelper helper, WorkItem workItem, User currentUser)
		{
			var isSubscribed = workItem.Subscriptions.Any(x => x.User.Id == currentUser.Id);
			var linkText = (isSubscribed ? "Unsubscribe" : "Subscribe");
			return "<button onclick=\"document.location.replace('/Fusion/{0}/{1}/{2}');\">{1} me!</button>".With(workItem.WorkItemType.GetDescription(), linkText, workItem.Id); ;
		}

		public static string SubscriptionList(this HtmlHelper helper, IList<Subscription> subscriptions)
		{
			var sb = new StringBuilder();
			var i = 1;
			foreach (var subscription in subscriptions)
			{
				sb.Append("<li class=\"subscription-list\">{0} {1}</li>".With(SubscriptionLink(helper, i++, subscription), SubscriptionDeleteImage(helper, subscription.Id)));
			}
			return (subscriptions.Any() ? sb.ToString() : "<li class=\"subscription-list\">No subscriptions</li>");
		}

		private static string SubscriptionDeleteImage(this HtmlHelper helper, int subscriptionId)
		{
			return helper.ImageLink("Unsubscribe", "/Areas/Fusion/Content/Images/icons/delete.png", "remove subscription", new { area = "Fusion", controller = "Profile", action = "Unsubscribe", id = subscriptionId }, new { title = "Unsubscribe" }, new { @class = "subscription-delete" });
		}

		private static string SubscriptionLink(this HtmlHelper helper, int i, Subscription subscription)
		{
			var type = subscription.WorkItem.WorkItemType.GetDescription();
			var tb = new TagBuilder("a") { InnerHtml = "<span>{0}. {1}: {2}</span>".With(i, type, subscription.WorkItem.Id) };
			tb.Attributes.Add("href", "/Fusion/{0}/Details/{1}".With(type, subscription.WorkItem.Id));
			return tb.ToString();
		}
	}
}
