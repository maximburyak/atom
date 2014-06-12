using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using log4net;

namespace Atom.Areas.Fusion.Events.Email
{
	public class Emailer
	{
		public Emailer(string SiteUrl)
		{
			siteurl = SiteUrl;
		}

		readonly private string siteurl;
		private readonly string _templateDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath) + "\\Email\\Templates\\App_Data";
		private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public void SendMailUsingTemplate(EmailerTemplate emailTemplate, MailMessage restOfEmail)
		{
			try
			{
				var physicalPath = String.Format("{0}\\{1}", _templateDirectory
												 , emailTemplate.TemplateFile);

				if (!File.Exists(physicalPath))
					throw new ArgumentException("Template file does not exist." + physicalPath, emailTemplate.TemplateFile);

				var emailBody = File.ReadAllText(physicalPath);
				foreach (KeyValuePair<string, string> kvp in emailTemplate.Tokens)
				{
					emailBody = emailBody.Replace(string.Format("##{0}##", kvp.Key.ToLower()), kvp.Value);
				}
				restOfEmail.Body = emailBody;

				if (siteurl.Contains("dev") || siteurl.Contains("localhost") || siteurl.Contains("training"))
				{
					restOfEmail.To.Clear();
					restOfEmail.CC.Clear();
					restOfEmail.Bcc.Clear();
					restOfEmail.To.Add("dev-system@bevalued.co.uk");
					restOfEmail.Subject = "DEVELOPMENT SITE: " + restOfEmail.Subject;
				}
				var client = new SmtpClient { Timeout = 10000 };
				client.Send(restOfEmail);
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
		}
	}
}