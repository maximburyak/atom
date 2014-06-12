using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class SupplierCategoryMappingV2PpClassMap : ClassMap<SupplierCategoryMappingV2Powerplay>
	{
		public SupplierCategoryMappingV2PpClassMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Category);
			Map(x => x.Format);
			Map(x => x.FormatType);
			Map(x => x.Class);
			Map(x => x.CreateDate).Column("CreatedDate");
			Map(x => x.CreatedBy);
			Table("Suppliers.dbo.SupplierCategoryMapping_V2_Powerplay");
		}
	}
}
