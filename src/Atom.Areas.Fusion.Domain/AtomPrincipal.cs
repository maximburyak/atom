using System;
using System.Security.Principal;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Areas.Fusion.Domain
{
	public class AtomPrincipal : GenericPrincipal
	{
		public DateTime Date { get; set; }
		public User User { get; set; }
		public AtomPrincipal(IIdentity identity, string[] roles)
			: base(identity, roles)
		{

		}
	}
}