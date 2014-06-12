using System.Net.Mail;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.WorkItem
{
	public class SubscriberRemoved : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
		public User Subscriber { get; set; }
	}

	public class SubscriberRemovedHandler : EventHandler, IEventHandler<SubscriberRemoved>
	{
		public void Handle(SubscriberRemoved args)
		{
			var type = args.WorkItem.WorkItemType.GetDescriptionOfEnum();
			var emailer = new Emailer(Host);
			var emailAddress = args.WorkItem.AlteredBy != null
								? args.WorkItem.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var message = new MailMessage(new MailAddress(emailAddress, "Fusion System"), new MailAddress(args.WorkItem.CreatedBy.EmailAddress))
							{
								Subject = string.Format("*{0} UPDATE* - Id {1}: {2}", args.WorkItem.WorkItemType.GetDescriptionOfEnum().ToUpper(), args.WorkItem.Id, args.WorkItem.Summary),
								IsBodyHtml = false
							};
			if (args.WorkItem.WorkItemType == WorkItemTypeEnum.Crf)
				message.CC.Add("changerequests@bevalued.co.uk");

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.WorkItem.WorkItemType.GetDescriptionOfEnum(), args.WorkItem.Id.ToString(), string.Format("https://{0}/Fusion/{1}/Details/{2}", Host, type, args.WorkItem.Id),
												 "Subscriber Removed", string.Format("This {0} has had a follower leave the flock: {1}", type, args.Subscriber.Name)), message);
		}
	}
}