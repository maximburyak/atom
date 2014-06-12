using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class FormatType_ClassMap : ClassMap<FormatType_Class>
	{
		public FormatType_ClassMap()
		{
			CompositeId()
				.KeyReference(x => x.FormatType)
				.KeyReference(x => x.Class);
			Table("ppd3.dbo.FormatType_Class_Map");
			ReadOnly();
		}
	}
}