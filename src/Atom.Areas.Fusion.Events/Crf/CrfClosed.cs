using System.Linq;
using System.Net.Mail;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Crf
{
	public class CrfClosed : IDomainEvent
	{
		public Domain.Models.Crf Crf { get; set; }
	}

	public class CrfClosedHandler : EventHandler, IEventHandler<CrfClosed>
	{
		public void Handle(CrfClosed args)
		{
			var emailer = new Emailer(Host);
			var emailAddress = args.Crf.AlteredBy != null
								? args.Crf.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var message = new MailMessage(new MailAddress(emailAddress, "Fusion System"), new MailAddress(args.Crf.CreatedBy.EmailAddress))
							{
								Subject = string.Format("*{0} CLOSED* - Id {1}: {2}", args.Crf.WorkItemType.GetDescriptionOfEnum().ToUpper(), args.Crf.Id, args.Crf.Summary),
								IsBodyHtml = false
							};

			message.CC.Add("changerequests@bevalued.co.uk");

			// Deal with any subscribers to this incident
			if (args.Crf.Subscriptions.Any())
			{
				foreach (var subscription in args.Crf.Subscriptions)
				{
					// Dont want to send subscription copy to same user as has been closed by
					if (subscription.User.EmailAddress != args.Crf.ClosedBy.EmailAddress)
					{
						message.CC.Add(subscription.User.EmailAddress);
					}
				}
			}

			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.Crf.WorkItemType.GetDescriptionOfEnum(), args.Crf.Id.ToString(), "https://" + Host + "/Fusion/" + args.Crf.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.Crf.Id,
												 "Crf Closed", string.Format("Crf has been closed by {0} on {1}", args.Crf.ClosedBy.Name, args.Crf.ClosedDate)), message);
		}
	}
}