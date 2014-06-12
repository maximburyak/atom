using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Atom.Areas.Fusion.Data.Conventions
{
	public class ImageConventions : IPropertyConvention
	{
		public void Apply(IPropertyInstance instance)
		{
			if (instance.Property.Name == "Data")
			{
				instance.CustomSqlType("image");
				instance.CustomType("BinaryBlob");
			}
		}
	}
}