using Atom.Main.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Main.Domain.NH.ClassMaps
{
	public sealed class AtomUserClassMap : ClassMap<AtomUser>
	{
		public AtomUserClassMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.UserID);
			Map(x => x.Name);
			Map(x => x.EmailAddress);
			Map(x => x.Team).Nullable();
			Map(x => x.UserFK);
			Table("AtomUserView");
			ReadOnly();
		}
	}
}