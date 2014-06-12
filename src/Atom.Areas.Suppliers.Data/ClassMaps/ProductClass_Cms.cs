using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class ProductClass_Cms : ClassMap<ClassCms>
	{
		public ProductClass_Cms()
		{
			Id(x => x.Class).Column("Class").GeneratedBy.Assigned();
			Map(x => x.Description);
			Table("Suppliers.dbo.Class_CMS");
			ReadOnly();
		}
	}
}
