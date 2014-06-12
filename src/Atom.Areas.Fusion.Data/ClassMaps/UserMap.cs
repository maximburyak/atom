using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class UserMap : ClassMap<User>, IClassMap
	{
		public UserMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.UserID);
			Map(x => x.Name);
			Map(x => x.EmailAddress);
			Map(x => x.Team).Nullable();
			//Map(x => x.UserFK);

			References(x => x.Department).Column("Department_id");
			Map(x => x.AccessLevel).CustomType(typeof(AccessLevelEnum));
			HasOne(x => x.Profile);
			Table("dbo.UserViewCopy");
			ReadOnly();
		}
	}
}