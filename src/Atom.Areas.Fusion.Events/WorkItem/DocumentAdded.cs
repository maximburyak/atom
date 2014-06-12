using System.Net.Mail;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.WorkItem
{
	public class DocumentAdded : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
		public User User { get; set; }
	}

	public class DocumentAddedHandler : EventHandler, IEventHandler<DocumentAdded>
	{
		public void Handle(DocumentAdded args)
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

			// Assigned to email address needs a copy (if not person who raised it!)
			if (args.WorkItem.AssignedTo != null)
			{
				if (args.WorkItem.AssignedTo.EmailAddress != args.WorkItem.CreatedBy.EmailAddress && args.WorkItem.AssignedTo != args.WorkItem.AlteredBy)
					message.CC.Add(args.WorkItem.AssignedTo.EmailAddress);
			}

			if (args.WorkItem.WorkItemType == WorkItemTypeEnum.Crf)
				message.CC.Add("changerequests@bevalued.co.uk");

			string caselink = "https://" + Host + "/Fusion/" + type + "/Details/" + args.WorkItem.Id;
			emailer.SendMailUsingTemplate(
				new WorkItemUpdatedEmailTemplate(args.WorkItem.WorkItemType.GetDescriptionOfEnum(), args.WorkItem.Id.ToString(),
					caselink,
					"Document Added", string.Format("This {0} has had a new document uploaded by: {1}", type, args.User.Name)), message);
		}
	}
}
