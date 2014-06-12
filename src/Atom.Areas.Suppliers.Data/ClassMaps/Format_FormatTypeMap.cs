using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
	public sealed class Format_FormatTypeMap : ClassMap<Format_FormatType>
	{
		public Format_FormatTypeMap()
		{
			CompositeId()
				.KeyReference(x => x.FormatType)
				.KeyReference(x => x.Format);
			Table("ppd3.dbo.Format_FormatType_Map");
			ReadOnly();
		}
	}
}