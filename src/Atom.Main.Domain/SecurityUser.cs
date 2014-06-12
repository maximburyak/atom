namespace Atom.Main.Domain
{
	public class SecurityUser
	{
		public virtual int id { get; set; }
		public virtual int userFK { get; set; }
		public virtual string userid { get; set; }
		public virtual string FName { get; set; }
		public virtual string SName { get; set; }
		public virtual string emailAddress { get; set; }
		public virtual int applicationAccess { get; set; }
		public virtual string FullName { get; set; }
	}
}
