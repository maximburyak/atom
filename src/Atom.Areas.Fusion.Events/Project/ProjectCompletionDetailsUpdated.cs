using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Project
{
	public class ProjectCompletionDetailsUpdated : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class ProjectCompletionDetailsUpdatedHandler : EventHandler, IEventHandler<ProjectCompletionDetailsUpdated>
	{
		public void Handle(ProjectCompletionDetailsUpdated args)
		{
			var emailer = new Emailer(Host);
			var emailAddress = args.WorkItem.AlteredBy != null
								? args.WorkItem.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var message = new MailMessage(new MailAddress(emailAddress, "Fusion System"), new MailAddress(args.WorkItem.CreatedBy.EmailAddress))
							{
								Subject = string.Format("*{0} UPDATE* - Id {1}: {2}", args.WorkItem.WorkItemType.GetDescriptionOfEnum().ToUpper(), args.WorkItem.Id, args.WorkItem.Summary),
								IsBodyHtml = false
							};

			//Deal with any subscribers to this incident
			if (args.WorkItem.Subscriptions.Any())
			{
				foreach (var subscription in args.WorkItem.Subscriptions)
				{
					// Dont want to send subscription copy to same user as has been assigned to
					if (subscription.User.EmailAddress != args.WorkItem.AssignedTo.EmailAddress)
					{
						message.CC.Add(subscription.User.EmailAddress);
					}
				}
			}

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.WorkItem.WorkItemType.GetDescriptionOfEnum(), args.WorkItem.Id.ToString(), "https://" + Host + "/Fusion/" + args.WorkItem.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.WorkItem.Id,
												 "Crf Completion Details Update", string.Format("Work Item completion details have been updated by {0} on {1}", args.WorkItem.AlteredBy.Name, args.WorkItem.AlteredDate)), message);
		}
	}
}