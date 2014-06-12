using System.Collections.Generic;

namespace Atom.Areas.Fusion.Events.Email
{
	public abstract class EmailerTemplate
	{
		public abstract string TemplateFile {get;}
		public abstract IDictionary<string, string> Tokens { get; }
        
	}
}