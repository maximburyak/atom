using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class FormatMap : ClassMap<Format>
	{
		public FormatMap()
		{
			Id(x => x.FormatCode).Column("Format").GeneratedBy.Assigned().UnsavedValue("");
			Map(x => x.Description);
			Map(x => x.SearchType);
			Map(x => x.ListEnable);
			Map(x => x.NotSearchable);
			Map(x => x.Seq);
			Map(x => x.Col);
			Map(x => x.SuperFmt);
			Map(x => x.Itemised);
			Map(x => x.AllowanceIgnore);
			Map(x => x.Colour);
			HasMany(x => x.FormatTypes)
				.KeyColumn("Format");
			Table("ppd3.dbo.Format");
			ReadOnly();
		}
	}
}
