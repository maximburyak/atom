using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Atom.Areas.Suppliers.Data.Conventions
{
	public class ClassConventions : IClassConvention
	{
		public void Apply(IClassInstance instance)
		{
			instance.DynamicUpdate();
		}
	}
}