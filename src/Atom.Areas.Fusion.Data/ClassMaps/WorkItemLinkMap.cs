using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class WorkItemLinkClassMap : ClassMap<WorkItemLink>, IClassMap
	{
		public WorkItemLinkClassMap()
		{
			Id(x => x.Id).GeneratedBy.Native(); 
			References(x => x.Item);
			References(x => x.RelatesTo);
			References(x => x.AlteredBy).Column("AlteredBy");
			References(x => x.CreatedBy).Column("CreatedBy");
			Map(x => x.CreateDate);
			Map(x => x.AlteredDate);

		}
	}
}