using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class SupplierClassMap : ClassMap<SupplierCms>
	{
		SupplierClassMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Name);
			Map(x => x.NonPpSupplier);
			Table("Suppliers.dbo.Suppliers_CMS");
			ReadOnly();
		}
	}
}
