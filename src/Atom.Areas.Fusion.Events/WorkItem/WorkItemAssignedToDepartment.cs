using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.WorkItem
{
	public class WorkItemAssignedToDepartment : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class WorkItemAssignedToDepartmentHandler : EventHandler, IEventHandler<WorkItemAssignedToDepartment>
	{
		public void Handle(WorkItemAssignedToDepartment args)
		{
			var emailer = new Emailer(Host);
			var emailAddress = args.WorkItem.AlteredBy != null
								? args.WorkItem.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var message = new MailMessage(new MailAddress(emailAddress, "Fusion System"), new MailAddress(args.WorkItem.Department.Email))
							{
								Subject = string.Format("*{0} UPDATE* - Id {1}: {2}", args.WorkItem.WorkItemType.GetDescriptionOfEnum().ToUpper(), args.WorkItem.Id, args.WorkItem.Summary),
								IsBodyHtml = false
							};

			// Raised by person needs to be kept in the loop
			message.CC.Add(args.WorkItem.CreatedBy.EmailAddress);
			if (args.WorkItem.WorkItemType == WorkItemTypeEnum.Crf)
				message.CC.Add("changerequests@bevalued.co.uk");

			//Deal with any subscribers to this incident
			if (args.WorkItem.Subscriptions != null)
			{
				if (args.WorkItem.Subscriptions.Any())
				{
					foreach (var subscription in args.WorkItem.Subscriptions)
					{
						// Dont want to send subscription copy to same user as has been assigned to
						if (subscription.User.EmailAddress != args.WorkItem.Department.Email)
						{
							message.CC.Add(subscription.User.EmailAddress);
						}
					}
				}
			}

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.WorkItem.WorkItemType.GetDescriptionOfEnum(), args.WorkItem.Id.ToString(), "https://" + Host + "/Fusion/" + args.WorkItem.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.WorkItem.Id,
												 "Work Item Department Assignment", string.Format("Work Item has been assigned to Department: {0} by {1}", args.WorkItem.Department.Description, args.WorkItem.AlteredBy.Name)), message);
		}
	}
}