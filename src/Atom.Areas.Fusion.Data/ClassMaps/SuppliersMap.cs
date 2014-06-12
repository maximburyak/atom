using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class SuppliersMap : ClassMap<Supplier>, IClassMap
	{
		public SuppliersMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Name);
			Map(x => x.DirectFulfil);
			Map(x => x.OrderBy);
			Table("Fusion.dbo.SuppliersView");
			ReadOnly();
		}
	}
}