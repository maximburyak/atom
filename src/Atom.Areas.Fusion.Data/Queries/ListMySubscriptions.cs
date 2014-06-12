using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListMySubscriptions : IQuery
	{
		public User user { get; set; }
		public bool showOpenOnly { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(Subscription));

			WorkItem WorkItem = null;

			if(showOpenOnly)
			{
				if (criteria.GetCriteriaByAlias(() => WorkItem) == null)
					criteria.CreateAlias<Subscription>(x => x.WorkItem, () => WorkItem);
				criteria.Add<Subscription>(x => x.WorkItem.WorkStatus < WorkItemStatus.Closed);
			}

			criteria.Add<Subscription>(x => x.User == user)
				.AddOrder<Subscription>(x => x.WorkItem.Id, Order.Desc)
				.SetMaxResults(200);
			return criteria;
		}
	}
}