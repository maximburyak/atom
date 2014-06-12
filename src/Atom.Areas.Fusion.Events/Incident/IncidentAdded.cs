using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Incident
{
	public class IncidentAdded : IDomainEvent
	{
		public SupportIncident Incident { get; set; }
	}

	public class IncidentAddedHandler : EventHandler, IEventHandler<IncidentAdded>
	{
		public void Handle(IncidentAdded args)
		{
			var emailer = new Emailer(Host);
			var from = new MailAddress(args.Incident.CreatedBy.EmailAddress, "Fusion System");
			var to = new MailAddress(args.Incident.System.Area.HandlingDepartment.Email);

			var message = new MailMessage(from, to)
							{
								Subject = string.Format("Id {0}: {1}", args.Incident.Id, args.Incident.Summary),
								IsBodyHtml = false
							};
			// Add CC in to the user who raised the case.
			message.CC.Add(args.Incident.CreatedBy.EmailAddress);

			var area = args.Incident.System.Area.Description ?? "";
			var category = (args.Incident.System.Category == null) ? "" : (args.Incident.System.Category.Description ?? "");
			var caselink = "https://" + Host + "/Fusion/Incident/Details/" + args.Incident.Id;

			// Deal with any subscribers to this incident
			if (args.Incident.Subscriptions.Any())
			{
				foreach (var subscription in
					args.Incident.Subscriptions.Where(subscription => subscription.User.EmailAddress != args.Incident.CreatedBy.EmailAddress))
				{
					message.CC.Add(subscription.User.EmailAddress);
				}
			}

			emailer.SendMailUsingTemplate(
				new IncidentAddedEmailTemplate(
					args.Incident.Id.ToString(),
					caselink,
					DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"), args.Incident.CreatedBy.Name,
					args.Incident.Summary,
					args.Incident.Location.Name,
					args.Incident.Severity.ToString(),
					area, category,
					EventHelpers.AdditionalInfoToString(args.Incident.AdditionalInfo),
					Regex.Replace(args.Incident.Comments[0].CommentText, "<(.|\n)*?>", "")
				)
			, message);
		}
	}
}