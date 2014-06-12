using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class InscoMap : ClassMap<InsuranceCompany>, IClassMap
	{
		public InscoMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Name);
			Map(x => x.DisplayName);
			Map(x => x.OrderBy);
			Table("Fusion.dbo.InsuranceCompanyView");
			ReadOnly();
		}
	}
}