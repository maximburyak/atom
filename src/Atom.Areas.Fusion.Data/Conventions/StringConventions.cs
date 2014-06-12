using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Atom.Areas.Fusion.Data.Conventions
{
	public class StringConventions : IPropertyConvention
	{
		public void Apply(IPropertyInstance instance)
		{
			if (instance.Property.Name == "CommentText")
			{
				instance.CustomSqlType("text");
			}
		}
	}
}