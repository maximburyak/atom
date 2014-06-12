using System.Collections.Generic;
using Atom.Main.Domain;

namespace Atom.Main.Models.ViewModels
{
	public class SecurityAdminViewModel
	{
		public IList<SecurityUser> AtomUsers { get; set; }
		public IList<SecurityUser> WildUsers { get; set; }
		public IList<SecurityUser> AllUsers { get; set; }
		public IList<SecurityRole> HasRoles { get; set; }
		public IList<SecurityRole> HaveRoles { get; set; }
	}
}
