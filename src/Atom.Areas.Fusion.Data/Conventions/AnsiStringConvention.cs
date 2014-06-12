using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Atom.Areas.Fusion.Data.Conventions
{
	public class AnsiStringConvention : IPropertyConvention, IPropertyConventionAcceptance
	{
		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType("AnsiString");
		}

		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Type == typeof(string));
		}
	}

	public class AnsiStringIdConvention : IIdConvention, IIdConventionAcceptance
	{
		public void Apply(IIdentityInstance instance)
		{
			instance.CustomType("AnsiString");
		}

		public void Accept(IAcceptanceCriteria<IIdentityInspector> criteria)
		{
			criteria.Expect(x => x.Type == typeof(string));
		}
	}
}
