using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.WorkItem
{
	public class WorkItemAssigned : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class WorkItemAssignedHandler : EventHandler, IEventHandler<WorkItemAssigned>
	{
		public void Handle(WorkItemAssigned args)
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
			var type = args.WorkItem.WorkItemType.GetDescriptionOfEnum();

			// Assigned to email address needs a copy (if not person who raised it!)
			if (args.WorkItem.AssignedTo.EmailAddress != args.WorkItem.CreatedBy.EmailAddress && args.WorkItem.AssignedTo != args.WorkItem.AlteredBy)
				message.CC.Add(args.WorkItem.AssignedTo.EmailAddress);

			//Deal with any subscribers to this incident
			if (args.WorkItem.Subscriptions != null)
			{
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
			}

			if (args.WorkItem.WorkItemType == WorkItemTypeEnum.Crf)
				message.CC.Add("changerequests@bevalued.co.uk");

			emailer.SendMailUsingTemplate(
			new WorkItemUpdatedEmailTemplate(type, args.WorkItem.Id.ToString(), "https://" + Host + "/Fusion/" + args.WorkItem.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.WorkItem.Id,
											 "Work Item Assignment", string.Format("Work Item has been assigned to: {0} by {1}", args.WorkItem.AssignedTo.Name, args.WorkItem.AlteredBy.Name)), message);
		}
	}

}