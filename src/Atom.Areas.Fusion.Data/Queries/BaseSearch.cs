using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class BaseSearch
	{
		public int Maxcase { get; set; }

		public DetachedCriteria GetQuery()
		{
			WorkItem workitem = null;
			var criteria = DetachedCriteria<WorkItem>.Create(() => workitem)
				//.AddOrder<WorkItem>(x => x.Id, Order.Desc)
				.SetFetchMode("System", FetchMode.Eager)
				.SetFetchMode("System.Area", FetchMode.Eager)
				.SetFetchMode("System.Category", FetchMode.Eager)
				.SetFetchMode("CreatedBy", FetchMode.Eager)
				.SetFetchMode("AssignedTo", FetchMode.Eager)
				.SetFetchMode("AssignedTo.Profile", FetchMode.Eager)
				.SetFetchMode("CreatedBy.Profile", FetchMode.Eager)
				.SetMaxResults(20);

			if (Maxcase > 0)
				criteria.Add<WorkItem>(x => x.Id < Maxcase);

			return criteria;
		}
	}
}
