using System;
using System.Security.Principal;
using System.Web.Security;

namespace Atom.Main.Domain.Models
{
	public interface IAtomUser
	{
		int Id { get; set; }
		string UserID { get; set; }
		string Name { get; set; }
		string Team { get; set; }
		string EmailAddress { get; set; }
		int UserFK { get; set; }
	}

	public class AtomUser : IAtomUser, IPrincipal
	{
		public virtual int Id { get; set; }
		public virtual string UserID { get; set; }
		public virtual string Name { get; set; }
		public virtual string Team { get; set; }
		public virtual string EmailAddress { get; set; }
		public virtual int UserFK { get; set; }
		public virtual bool IsInRole(string role)
		{
		    throw new NotSupportedException("User RoleManager.IsUserInRole instead.");
		}

		public virtual IIdentity Identity
		{
			get { return new Identity(true, UserID); }
		}
	}
}

