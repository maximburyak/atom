using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class SupplierCategoryMappingCmsClassMap : ClassMap<SupplierCategoryMappingCms>
	{
		public SupplierCategoryMappingCmsClassMap()
		{
			CompositeId()
				.KeyProperty(x => x.SupplierID)
				.KeyProperty(x => x.Level1)
				.KeyProperty(x => x.Level2)
				.KeyProperty(x => x.Level3)
				.KeyProperty(x => x.Level4)
				.KeyProperty(x => x.Format)
				.KeyProperty(x => x.FormatType);
			
            Map(x => x.Class);
			Map(x => x.Disabled);
            Map(x => x.Ignored);
			Map(x => x.CreateDate).Nullable();
			Map(x => x.CreatedBy).Nullable();

            Table("Suppliers.dbo.SupplierCategoryMapping_CMS");
		}
	}
}
