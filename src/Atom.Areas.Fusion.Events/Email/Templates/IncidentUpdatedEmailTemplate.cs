using System.Collections.Generic;

namespace Atom.Areas.Fusion.Events.Email.Templates
{
	public class WorkItemUpdatedEmailTemplate : EmailerTemplate
	{
		const string _TemplateFile = "WorkItemUpdated.txt";
		private readonly Dictionary<string, string> _Tokens;

		public WorkItemUpdatedEmailTemplate(string workitemtype, string caseid, string caselink, string updatetype, string update)
		{
			_Tokens = new Dictionary<string, string>
			          	{
			          		{"workitemtype", workitemtype},
			          		{"caseid", caseid},
			          		{"caselink",caselink},
							{"updatetype", updatetype},
			          		{"caseupdate", update}
			          	};
		}

		public override string TemplateFile
		{
			get { return _TemplateFile; }
		}

		public override IDictionary<string, string> Tokens
		{
			get { return _Tokens; }
		}
	}
}