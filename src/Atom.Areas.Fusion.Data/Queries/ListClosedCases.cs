using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListClosedCases : IQuery
	{
		public int maxcase { get; set; }
		public ICriteria GetQuery(ISession session, WorkItemTypeEnum? workItem)
		{
			var criteria = session.CreateCriteria(typeof(WorkItem));

			if (workItem != null)
				criteria.Add<WorkItem>(x => x.WorkItemType == workItem);

			if (maxcase > 0)
				criteria.Add<WorkItem>(x => x.Id < maxcase);

			criteria.Add<WorkItem>(x => x.WorkStatus == WorkItemStatus.Closed)
					.AddOrder<WorkItem>(x => x.CreateDate, Order.Desc)
					.SetMaxResults(20)
					.SetFetchModes();
			return criteria;
		}

		public ICriteria GetQuery(ISession session)
		{
			return GetQuery(session, WorkItemTypeEnum.Incident);
		}
	}
}
