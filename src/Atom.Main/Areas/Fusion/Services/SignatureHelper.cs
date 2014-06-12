using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Services.Domain;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class SignatureHelper
	{
		private const string _missingImage = "missing.gif";

		public static string WorkItemChangeBoardSignOff(this HtmlHelper helper, WorkItem item, User user)
		{
			// If no workitem approval, show nothing
			if (!item.SignOffs.Any(x => x.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance)) return "No signature to show";

			if (item.Rejected.HasValue) return "This {0} was rejected {1}".With(item.WorkItemType.GetDescription(), item.Rejected.Value.FormatDateTimeRelative());

			var sb = new StringBuilder();
			var signOff = item.SignOffs.Where(x => x.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance).First();
			sb.Append("<ul class=\"cbasignature\"><li><h3 style=\"padding-bottom:2px;\">Signature</h3>");
			//sb.Append(helper.SignatureForWorkItem(signOff, new { @class = "signature", style = "float:left;" }));
			sb.Append("<div class=\"signoff-item\">");

			if (signOff.SignedOff.HasValue)
			{
				sb.Append("<span style=\"font-size:8pt;color:#F79521\">Signed off by {0}, {1}</span>".With(signOff.SignedOffBy.Name, signOff.SignedOff.FormatDateTimeRelative()));
			}
			else if (item.WorkStatus == WorkItemStatus.AwaitingApproval)
			{
				sb.Append(SignatureWorkItemApprove(helper, user, item, signOff));
			}
			sb.Append("</div></li><div class=\"clear\"></div><br/></ul>");

			return sb.ToString();
		}

		public static string WorkItemEmergencySignOff(this HtmlHelper helper, WorkItem item, User user)
		{
			if (!item.SignOffs.Any(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance)) return "No signatures to show";

			if (item.Rejected.HasValue) return "This {0} was rejected {1}".With(item.WorkItemType.GetDescription(), item.Rejected.Value.FormatDateTimeRelative());

			var sb = new StringBuilder();
			var items = item.SignOffs.Where(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance);
			sb.Append("<div>");
			sb.Append("<h3 style=\"padding-bottom:4px;\">Signature(s)</h3>");
			sb.Append("<ul>");

			var i = 1;
			foreach (var signOff in items)
			{
				sb.Append("<li><div class=\"signoff-item\">");
				sb.Append("<span>{0}.{1}</span><br/>".With(i++, signOff.SignOffType.GetDescription()));
				if (signOff.SignedOff.HasValue)
				{
					sb.Append("<span style=\"font-size:8pt;color:#F79521\">Signed off by {0}, {1}</span>".With(signOff.SignedOffBy.Name, signOff.SignedOff.FormatDateTimeRelative()));
				}
				else
				{
					if (item.WorkStatus == WorkItemStatus.AwaitingApproval)
					{
						sb.Append(SignatureWorkItemEmergencySignOffApprove(helper, user, item, signOff));
					}
				}

				sb.Append("</div></li>");

			}
			sb.Append("</ul></div><br/>");
			return sb.ToString();
		}

		public static string SignatureList(this HtmlHelper helper, User user)
		{
			if (user.Profile == null || user.Profile.Signatures == null)
				return "You have not yet uploaded a signature image.";

			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			var ul = new TagBuilder("ul");
			var li = new TagBuilder("li");
			foreach (var a in user.Profile.Signatures)
			{
				if (a.Id == user.Profile.CurrentSignature) continue;
				li.InnerHtml = helper.ImageTag(urlHelper.Signature("{0}_sig_{1}.gif".With(user.UserID, a.Id)), user.Name, user.Name, new { @class = "signature" });
				li.InnerHtml += helper.RadioButton("CurrentSignature", a.Id, false);
				ul.InnerHtml += li.ToString();
			}
			return ul.ToString();
		}

		public static string RequiredSignOffList(this HtmlHelper helper, User user, IList<WorkItemSignOff> required, WorkItem workItem)
		{
			if (user.Profile == null || user.Profile.Signatures == null)
				return "You have not yet uploaded a signature image.";

			var noSignatures = (!user.Profile.Signatures.Any());

			var sb = new StringBuilder();
			var i = 1;
			sb.Append("<ul class=\"cbasignature\">");
			foreach (var rs in required.OrderBy(x => x.SignOffType))
			{
				sb.Append("<li>");
				var id = rs.ToString();
				var haveSignature = (workItem.SignOffs.Any(x => x.SignOffType == rs.SignOffType && rs.SignedOff != null));

				//sb.Append(helper.SignatureForWorkItem(rs, new { @class = "signature", style = "float:left;" }));
				sb.Append("<div class=\"signoff-item\"><span class=\"signoff-type\">" + i++ + ". " + rs.SignOffType.GetDescription() + "</span> ");
				if (haveSignature)
				{
					sb.Append("<span class=\"signoff-outcome\">Signed off by {0}, {1} ({2})".With(rs.SignedOffBy.Name, rs.SignedOff.HasValue ? rs.SignedOff.Value.ToString("dddd, dd MMMM yyyy HH:mm:ss") : "", rs.SignedOff.FormatDateTimeRelative()) + "</span>");
				}
				else
				{
					if (!noSignatures)
					{
						sb.Append("<span class=\"signoff-outcome\">" + SignatureAssignLink(helper, user, workItem, rs) + "</span>");
					}
					else
					{
						sb.Append(NoSignaturesLink(helper));
					}
				}
				sb.Append("</div></li><div class=\"clear\"></div><br/>");
			}
			sb.Append("</ul>");

			return sb.ToString();
		}

		private static string SignatureAssignLink(HtmlHelper helper, User user, WorkItem item, WorkItemSignOff rs)
		{
			if (RoleAuthorizationService.WorkItemITSignOffs(rs.SignOffType)) return SignatureError("Only IT can sign this off");
			if (RoleAuthorizationService.WorkItemBusinessSignOffs(rs.SignOffType) && !(user.Id == item.CreatedBy.Id)) return SignatureError("Only Senior Management or the Owner can sign this off");

			// Business testing can be signed off by anyone (at this point they have gone through role check to ensure they can see signatures.
			return helper.ActionLink("Sign off by me ({0})".With(user.Name), "SignOff", item.WorkItemType.GetDescription(), new { id = rs.Id }, null).ToString();
		}

		private static string SignatureWorkItemApprove(HtmlHelper helper, User user, WorkItem item, WorkItemSignOff rs)
		{
			return !RoleAuthorizationService.WorkItemCanApprove(item.WorkItemType) ? SignatureError("Only Senior Management can sign this off") : helper.ActionLink("Sign off by me ({0})".With(user.Name), null, null, null, new { id = "workitemapprove", href = "#" }).ToString();
		}

		private static string SignatureWorkItemEmergencySignOffApprove(HtmlHelper helper, User user, WorkItem item, WorkItemSignOff rs)
		{
			return !RoleAuthorizationService.CrfEmergencySignOffCanApprove(item.WorkItemType)
					? // Cant do it
				   SignatureError("Only the Change Board can sign this off")
					:
				//Can do it
				   SignatureWorkItemEmergencySignOff(helper, user, item, rs);

		}

		private static string SignatureWorkItemEmergencySignOff(HtmlHelper helper, User user, WorkItem item, WorkItemSignOff signOff)
		{
			// Here we need to check
			// 1. If the user can sign it off
			// 1.1 by seeing if they have already signed off another one
			// 2. If not, create a link saying SignOffEmergency etc
			var hasSignedOffEmergencyItem = item.SignOffs.Any(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance && x.SignedOffBy == user);
			if (hasSignedOffEmergencyItem)
				return SignatureError("Another member of the Board must sign this off.");

			return helper.ActionLink("Sign off by me ({0})".With(user.Name), "EmergencySignOff", "Crf", new { id = signOff.Id }, null).ToString();
		}

		private static string SignatureError(string message)
		{
			return "<span class=\"signature-error\" >{0}</span>".With(message);
		}

		private static string NoSignaturesLink(HtmlHelper helper)
		{
			return helper.ActionLink("Create Signature", "CreateSignature", "Profile", new { }, new { @class = "signoff-createsig" }).ToString();
		}

		public static string Signature(this HtmlHelper helper, User user, object imageHtmlAttributes)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			if (user.Profile == null || user.Profile.CurrentSignature == 0)
				return helper.ImageTag(urlHelper.Signature(_missingImage), "Signature not set", "Signature missing", imageHtmlAttributes);

			return helper.ImageTag(urlHelper.Signature(String.Format("{0}_sig_{1}.gif", user.UserID, user.Profile.CurrentSignature)), user.Name, user.Name, imageHtmlAttributes);
		}

		private static string SignatureForWorkItem(this HtmlHelper helper, WorkItemSignOff signOff, object imageHtmlAttributes)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
			if (signOff.SignedOffBy == null)
				return helper.ImageTag(urlHelper.Signature(_missingImage), "Signature not set", "Signature missing", imageHtmlAttributes);

			return helper.ImageTag(urlHelper.Signature(String.Format("{0}_sig_{1}.gif", signOff.SignedOffBy.UserID, signOff.SignedOffBy.Profile.CurrentSignature)), signOff.SignedOffBy.Name, signOff.SignedOffBy.Name, imageHtmlAttributes);
		}
	}

}
