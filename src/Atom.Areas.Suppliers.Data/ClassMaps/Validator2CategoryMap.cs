using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class Validator2CategoryMap : ClassMap<Validator2Category>
	{
		public Validator2CategoryMap()
		{
			Id(x => x.Category).Column("Supplierviewcategory").GeneratedBy.Assigned().UnsavedValue("Unknown");
			Table("Suppliers.dbo.Validator2Categories");
			ReadOnly();
		}
	}
}