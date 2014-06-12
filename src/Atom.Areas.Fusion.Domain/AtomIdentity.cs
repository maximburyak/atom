using System.Security.Principal;

namespace Atom.Areas.Fusion.Domain
{
	public class AtomIdentity : GenericIdentity
	{
		public AtomIdentity(string name) : base(name) { }
	}
}
