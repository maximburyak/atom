using System.Collections.Generic;
using System.Net.Mail;
using BeValued.Utilities.Extensions;

namespace Atom.Areas.Fusion.Events
{
	public class AtomMailMessage : MailMessage
	{
		public AtomMailMessage(MailAddress @from, MailAddress to) : base(@from, to) { }

		public void AddCc(IList<string> addresses)
		{
			addresses.ForEach(x => CC.Add(x));
		}
	}
}
