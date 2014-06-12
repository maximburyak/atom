using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class FilterMap : ClassMap<Filter>, IClassMap
	{
		public FilterMap()
		{
			Id(x => x.Id).GeneratedBy.Native(); 
			Map(x => x.Description);
			Map(x => x.FilterValue);
			Map(x => x.IsDefault);
			Map(x => x.AlteredDate);
			References(x => x.AlteredBy).Column("AlteredBy");
			References(x => x.CreatedBy).Column("CreatedBy");
			Map(x => x.CreateDate);
			Table("Fusion.dbo.Filter");
		}
	}
}