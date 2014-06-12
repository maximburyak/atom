using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class PowerplaySuppliedFeedItemMap : ClassMap<PowerplaySuppliedFeedItem>
	{
		public PowerplaySuppliedFeedItemMap()
		{
			Id(x => x.ProdId).GeneratedBy.Native();
			Map(x => x.SupplierId);
			References(x => x.Format).Column("Format");
			References(x => x.FormatType).Column("FormatType");
			References(x => x.Class).Column("Class");
			Table("Suppliers.dbo.POWERPLAY");
		}
	}
}