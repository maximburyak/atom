using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Crf
{
	public class CrfCompletionDetailsUpdated : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class CrfCompletionDetailsUpdatedHandler : EventHandler, IEventHandler<CrfCompletionDetailsUpdated>
	{
		public void Handle(CrfCompletionDetailsUpdated args)
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
			message.CC.Add("changerequests@bevalued.co.uk");

			//Deal with any subscribers to this incident
			var subscriptions = args.WorkItem.Subscriptions
				.Where(s => (args.WorkItem.AssignedTo != null) && s.User.EmailAddress != args.WorkItem.AssignedTo.EmailAddress);

			foreach (var s in subscriptions)
				message.CC.Add(s.User.EmailAddress);

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.WorkItem.WorkItemType.GetDescriptionOfEnum(), args.WorkItem.Id.ToString(), "https://" + Host + "/Fusion/" + args.WorkItem.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.WorkItem.Id,
												 "Crf Completion Details Update", string.Format("Work Item completion details have been updated by {0} on {1}", args.WorkItem.AlteredBy.Name, args.WorkItem.AlteredDate)), message);
		}
	}
}