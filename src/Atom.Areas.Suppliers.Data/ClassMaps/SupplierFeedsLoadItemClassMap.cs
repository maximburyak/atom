using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class SupplierFeedsLoadItemClassMap : ClassMap<SupplierFeedsLoadItem>
	{
		public SupplierFeedsLoadItemClassMap()
		{
			CompositeId()
				.KeyProperty(x => x.SupplierId)
				.KeyProperty(x => x.Level1)
				.KeyProperty(x => x.Level2)
				.KeyProperty(x => x.Level3)
				.KeyProperty(x => x.Level4);
			ReadOnly();
			Table("Suppliers.dbo.SupplierFeeds_LOAD");
		}
	}
}