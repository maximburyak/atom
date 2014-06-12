using System.Collections.Generic;

namespace Atom.Areas.Fusion.Events.Email.Templates
{
	public class IncidentAddedEmailTemplate : EmailerTemplate
	{
		const string _TemplateFile = "IncidentAdded.txt";
		private readonly Dictionary<string, string> _tokens;

		public IncidentAddedEmailTemplate(string caseid, string caselink, string submitted, string submittedby,
		                                  string description, string location, string severity, 
		                                  string supportarea, string category, string additionalinfo, string summary)
		{
			_tokens = new Dictionary<string, string>
			          	{
			          		{"caseid", caseid},
			          		{"caselink", caselink},
			          		{"submitted", submitted},
			          		{"submittedby", submittedby},
			          		{"description", description},
			          		{"location", location},
			          		{"severity", severity},
			          		{"supportarea", supportarea},
			          		{"category", category},
			          		{"additionalinfo", additionalinfo},
			          		{"summary", summary}
			          	};
		}

		public override string TemplateFile
		{
			get { return _TemplateFile; }
		}

		public override IDictionary<string, string> Tokens
		{
			get { return _tokens; }
		}
	}
}