using System.Collections.Generic;

namespace Atom.Areas.Fusion.Events.Email.Templates
{
	public class CrfAddedEmailTemplate : EmailerTemplate
	{
		const string _TemplateFile = "CrfAdded.html";
		private readonly Dictionary<string, string> _tokens;

		public CrfAddedEmailTemplate(string crfid, string crflink, string submitted, string submittedby,
		                             string description, string inscos, string products,
		                             string otherscope, string businessbenefit, string alternatives, string summary, string severity, string suppliers, string clientreq)
		{
			_tokens = new Dictionary<string, string>
			          	{
			          		{"crfid", crfid},
			          		{"submitted", submitted},
			          		{"submittedby", submittedby},
			          		{"summary", summary},
			          		{"description", description},
			          		{"inscos", inscos},
			          		{"products", products},
			          		{"otherscope", otherscope},
			          		{"businessbenefit", businessbenefit},
			          		{"alternatives", alternatives},
			          		{"severity", severity},
			          		{"crflink", crflink},
			          		{"suppliers", suppliers},
			          		{"clientreq", clientreq}

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