using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class SupplierCategoryMappingV2ClassMap : ClassMap<SupplierCategoryMappingV2>
	{
		public SupplierCategoryMappingV2ClassMap()
		{
			CompositeId()
				.KeyProperty(x => x.SupplierID)
				.KeyProperty(x => x.Category)
				.KeyProperty(x => x.Level1)
				.KeyProperty(x => x.Level2)
				.KeyProperty(x => x.Level3);
			Map(x => x.Disabled);
			Map(x => x.CreateDate);
			Map(x => x.CreatedBy);
			Table("Suppliers.dbo.SupplierCategoryMapping_V2");
		}
	}
}
