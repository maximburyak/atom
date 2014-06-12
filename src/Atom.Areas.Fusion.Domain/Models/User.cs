using Atom.Main.Domain.Models;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class User : IAtomUser
	{
		public virtual int Id { get; set; }
		public virtual string UserID { get; set; }
		public virtual string Name { get; set; }
		public virtual AccessLevelEnum AccessLevel { get; set; }
		public virtual string Team { get; set; }
		public virtual SupportProfile Profile { get; set; }
		public virtual string EmailAddress { get; set; }
		public virtual int UserFK { get; set; }
		public virtual HandlingDepartment Department { get; set; }
	}
}