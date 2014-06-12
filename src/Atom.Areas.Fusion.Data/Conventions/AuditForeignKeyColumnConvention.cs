using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace Atom.Areas.Fusion.Data.Conventions
{
	public class AuditForeignKeyColumnConvention : ForeignKeyConvention
	{
		protected override string GetKeyName(Member property, Type type)
		{
			if (property == null)
				return type.Name + "_Id";

			var extension = (property.Name == "CreatedBy" || property.Name == "AlteredBy" ? "" : "_Id");

			return property.Name + extension;
		}
	}
}