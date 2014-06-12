using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListLinkedWorkItems : IQuery
	{
		public int Id { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			WorkItem workitem = null;
			var criteria = session.CreateCriteria<WorkItemLink>()
					.Add<WorkItemLink>(x => x.Item.Id == Id)
					.CreateAlias<WorkItemLink>(c => c.RelatesTo, () => workitem, JoinType.InnerJoin)
					.SetProjection(Projections.ProjectionList()
						.Add(LambdaProjection.Property<WorkItemLink>(x => x.Id), PropertyUtil.GetName<WorkItemLinkDto>(x => x.Id))
						.Add(LambdaProjection.Property<WorkItemLink>(x => workitem.WorkItemType), PropertyUtil.GetName<WorkItemLinkDto>(x => x.RelatesToType))
						.Add(LambdaProjection.Property<WorkItemLink>(x => x.Item.Id), PropertyUtil.GetName<WorkItemLinkDto>(x => x.FromWorkItemId))
						.Add(LambdaProjection.Property<WorkItemLink>(x => x.RelatesTo.Id), PropertyUtil.GetName<WorkItemLinkDto>(x => x.RelatesToWorkItemId))
					)
					.SetResultTransformer(Transformers.AliasToBean(typeof(WorkItemLinkDto)));
			return criteria;
		}
	}
}
