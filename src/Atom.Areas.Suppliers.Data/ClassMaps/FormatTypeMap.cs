using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class FormatTypeMap : ClassMap<FormatType>
	{
		public FormatTypeMap()
		{
			Id(x => x.FormatTypeCode).Column("FormatType").GeneratedBy.Assigned();
			Map(x => x.Description);
			Map(x => x.Col);
			Map(x => x.ListEnable);
			Map(x => x.NotSearchable);
			Map(x => x.Seq);
			Map(x => x.CategoryCode);
			HasMany(x => x.Classes).KeyColumn("FormatType");
			Table("ppd3.dbo.FormatType");
			ReadOnly();
		}
	}
}
