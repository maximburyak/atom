using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.WorkItem
{

	public class WorkItemSignOffCompleted : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class WorkItemSignOffCompletedHandler : EventHandler, IEventHandler<WorkItemSignOffCompleted>
	{
		public void Handle(WorkItemSignOffCompleted args)
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
					message.CC.Add(subscription.User.EmailAddress);
				}
			}

			//Add in copy to the user who just signed it off in case of fraudulent use
			var user = args.WorkItem.SignOffs.OrderBy(x => x.SignedOff).Last().SignedOffBy;
			message.CC.Add(user.EmailAddress);

			if (args.WorkItem.WorkItemType == WorkItemTypeEnum.Crf)
				message.CC.Add("changerequests@bevalued.co.uk");

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.WorkItem.WorkItemType.GetDescriptionOfEnum(), args.WorkItem.Id.ToString(), "http://" + Host + "/Fusion/" + args.WorkItem.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.WorkItem.Id,
					"Work Item Sign Off Confirmed", string.Format("Work Item has had the following sign off confirmed: {0} by {1} on {2}", args.WorkItem.SignOffs.OrderBy(x => x.SignedOff).Last().SignOffType, args.WorkItem.AlteredBy.Name, args.WorkItem.AlteredDate)), message);
		}

	}
}
