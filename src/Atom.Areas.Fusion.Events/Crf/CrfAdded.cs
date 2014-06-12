using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Atom.Areas.Fusion.Events.Email;
using Atom.Areas.Fusion.Events.Email.Templates;

namespace Atom.Areas.Fusion.Events.Crf
{
	public class CrfAdded : IDomainEvent
	{
		public Domain.Models.Crf Crf { get; set; }
		public Domain.Models.User CreatedCrfUser { get; set; }
	}

	public class CrfAddedHandler : EventHandler, IEventHandler<CrfAdded>
	{
		public void Handle(CrfAdded args)
		{
			var emailer = new Emailer(Host);
			var emailAddress = args.Crf.Department.Email;
			var message = new MailMessage(new MailAddress(args.CreatedCrfUser.EmailAddress, "Fusion System"), new MailAddress(emailAddress))
							{
								Subject = string.Format("Id {0}: {1}", args.Crf.Id, args.Crf.Summary),
								IsBodyHtml = true

							};
			// Add CC in to the user who raised the case.
			message.CC.Add(args.CreatedCrfUser.EmailAddress);
			message.CC.Add("changeboard@bevalued.co.uk");
			message.CC.Add("changerequests@bevalued.co.uk");

			var link = "https://" + Host + "/Fusion/" + args.Crf.WorkItemType.GetDescriptionOfEnum() + "/Details/" +
					   args.Crf.Id;

			emailer.SendMailUsingTemplate(
				new CrfAddedEmailTemplate(args.Crf.Id.ToString(), link,
										  DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"), args.CreatedCrfUser.Name, args.Crf.Summary,
										  string.Join(",",
													  (from c in args.Crf.InsuranceCompanies select c.InsuranceCompany.Name).ToArray
														()),
										  string.Join(",", (from c in args.Crf.ProductGroups select c.ProductGroup.Name).ToArray()),
										  args.Crf.OtherScope, args.Crf.BusinessBenefit, args.Crf.Alternatives,
										  Regex.Replace(args.Crf.Comments[0].CommentText, "<(.|\n)*?>", ""),
										  args.Crf.Severity.GetDescriptionOfEnum(), string.Join(",", (from c in args.Crf.Suppliers select c.Supplier.Name).ToArray()), args.Crf.ClientRequirement.ToString()), message);
		}
	}
}