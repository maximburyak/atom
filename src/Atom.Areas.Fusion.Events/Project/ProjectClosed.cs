using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Project
{
	public class ProjectClosed : IDomainEvent
	{
		public Domain.Models.Project Project { get; set; }
	}

	public class ProjectClosedHandler : EventHandler, IEventHandler<ProjectClosed>
	{
		public void Handle(ProjectClosed args)
		{
			var emailer = new Emailer(Host);
			var emailAddress = args.Project.AlteredBy != null
								? args.Project.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var message = new MailMessage(new MailAddress(emailAddress, "Fusion System"), new MailAddress(args.Project.CreatedBy.EmailAddress))
							{
								Subject = string.Format("*{0} CLOSED* - Id {1}: {2}", args.Project.WorkItemType.GetDescriptionOfEnum().ToUpper(), args.Project.Id, args.Project.Summary),
								IsBodyHtml = false
							};

			// Deal with any subscribers to this incident
			if (args.Project.Subscriptions.Any())
			{
				foreach (var subscription in args.Project.Subscriptions)
				{
					// Dont want to send subscription copy to same user as has been closed by
					if (subscription.User.EmailAddress != args.Project.ClosedBy.EmailAddress)
					{
						message.CC.Add(subscription.User.EmailAddress);
					}
				}
			}

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.Project.WorkItemType.GetDescriptionOfEnum(), args.Project.Id.ToString(), "https://" + Host + "/Fusion/" + args.Project.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.Project.Id,
												 "Project Closed", string.Format("Project has been closed by {0} on {1}", args.Project.ClosedBy.Name, args.Project.ClosedDate)), message);
		}
	}
}