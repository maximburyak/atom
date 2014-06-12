using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class ChangeBoardMap : ClassMap<ChangeBoard>, IClassMap
	{
		public ChangeBoardMap()
		{
			Id(x => x.Id).GeneratedBy.Native(); 
			Map(x => x.NextMeeting);
			Map(x => x.AlteredDate);
			References(x => x.AlteredBy).Column("AlteredBy");
			References(x => x.CreatedBy).Column("CreatedBy");
			Map(x => x.CreateDate);
			Table("Fusion.dbo.ChangeBoard");
		}
	}
}
