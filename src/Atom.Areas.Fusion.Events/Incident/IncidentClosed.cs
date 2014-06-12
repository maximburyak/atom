using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Incident
{
	public class IncidentClosed : IDomainEvent
	{
		public SupportIncident Incident { get; set; }
	}

	public class IncidentClosedHandler : EventHandler, IEventHandler<IncidentClosed>
	{
		public void Handle(IncidentClosed args)
		{
			var emailer = new Emailer(Host);
			var emailAddress = args.Incident.AlteredBy != null
								? args.Incident.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var message = new MailMessage(new MailAddress(emailAddress, "Fusion System"), new MailAddress(args.Incident.CreatedBy.EmailAddress))
							{
								Subject = string.Format("*{0} CLOSED* - Id {1}: {2}", args.Incident.WorkItemType.GetDescriptionOfEnum().ToUpper(), args.Incident.Id, args.Incident.Summary),
								IsBodyHtml = false
							};

			// Deal with any subscribers to this incident
			if (args.Incident.Subscriptions.Any())
			{
				foreach (var subscription in args.Incident.Subscriptions)
				{
					// Dont want to send subscription copy to same user as has been closed by
					if (subscription.User.EmailAddress != args.Incident.ClosedBy.EmailAddress)
					{
						message.CC.Add(subscription.User.EmailAddress);
					}
				}
			}

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.Incident.WorkItemType.GetDescriptionOfEnum(), args.Incident.Id.ToString(), "https://" + Host + "/Fusion/Incident/Details/" + args.Incident.Id,
												 "Incident Closure", string.Format("Incident has been closed by {0}\n\non {1}", args.Incident.ClosedBy.Name, args.Incident.ClosedDate)), message);
		}
	}
}