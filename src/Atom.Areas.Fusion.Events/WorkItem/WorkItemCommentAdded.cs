using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.WorkItem
{
	public class WorkItemCommentAdded : IDomainEvent
	{
		public Comment Comment { get; set; }
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class WorkItemCommentAddedHandler : EventHandler, IEventHandler<WorkItemCommentAdded>
	{
		public void Handle(WorkItemCommentAdded args)
		{
			var emailer = new Emailer(Host);
			var fromEmail = args.WorkItem.AlteredBy != null
								? args.WorkItem.AlteredBy.EmailAddress
								: "fusion-noreply@bevalued.co.uk";
			var from = new MailAddress(fromEmail, "Fusion System");

			//get email address to send to
			var emailAddress = args.WorkItem.EmailTo(args.Comment.CreatedBy);

			// No E-Mail address to send to so do nothing
			if (string.IsNullOrEmpty(emailAddress)) return;

			//initiate message
			var subject = string.Format("*{0} UPDATE* - Id {1}: {2}", args.WorkItem.WorkItemType.GetDescriptionOfEnum().ToUpper(),
										args.WorkItem.Id, args.WorkItem.Summary).Replace("\n", "").Replace("\r", "");
			var message = new AtomMailMessage(from, new MailAddress(emailAddress))
			{
				Subject = subject,
				IsBodyHtml = false
			};

			//add any additional email addresses
			message.AddCc(args.WorkItem.EmailCc(args.Comment.CreatedBy));

			//send message
			emailer.SendMailUsingTemplate(new WorkItemUpdatedEmailTemplate(
					args.WorkItem.WorkItemType.GetDescriptionOfEnum(),
					args.WorkItem.Id.ToString(),
					"https://" + Host + "/Fusion/" + args.WorkItem.WorkItemType.GetDescriptionOfEnum() + "/Details/" + args.WorkItem.Id,
					"Work Item Update",
					string.Format(
						"Comment Added: {0}\n\nby {1}\n\non {2}",
						Regex.Replace(args.Comment.CommentText, "<(.|\n)*?>", ""),
						args.Comment.CreatedBy.Name,
						DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt")
					)
				)
			, message);
		}
	}
}