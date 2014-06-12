
using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Atom.Areas.Fusion.Data.Queries
{
	//public class ListMyOpenCases : IQuery
	//{
	//    public User user { get; set; }
	//    public int maxcase { get; set; }
	//    public ICriteria GetQuery(ISession session, WorkItemTypeEnum? type)
	//    {
	//        var criteria = session.CreateCriteria(typeof(WorkItem));

	//        if (type != null)
	//            criteria.Add<WorkItem>(x => x.WorkItemType == type);

	//        if (maxcase > 0)
	//            criteria.Add<WorkItem>(x => x.Id < maxcase);

	//        criteria.Add<WorkItem>(x => x.WorkStatus < WorkItemStatus.Closed)
	//                .Add(Restrictions.Or(
	//                        SqlExpression.CriterionFor<WorkItem>(x => x.CreatedBy == user),
	//                        SqlExpression.CriterionFor<WorkItem>(x => x.AssignedTo == user)
	//                ))
	//                .AddOrder<WorkItem>(x => x.WorkItemType, Order.Asc)
	//                .AddOrder<WorkItem>(x => x.CreateDate, Order.Desc)
	//                .AddOrder<WorkItem>(x => x.Id, Order.Desc)
	//                .SetMaxResults(20)
	//                .SetFetchModes();
	//        return criteria;
	//    }

	//    public ICriteria GetQuery(ISession session)
	//    {
	//        return GetQuery(session, WorkItemTypeEnum.Incident);
	//    }
	//}

	public abstract class ListMyOpenCasesBaseQuery
	{
		public virtual IQueryOver<WorkItem, WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, User user)
		{
			var subQuery = QueryOver.Of<WorkItem>()
				.And(x => x.WorkStatus < WorkItemStatus.Closed)
				.Select(x => x.Id)
				.OrderBy(x => x.WorkItemType).Asc
				.OrderBy(x => x.CreateDate).Desc
				.OrderBy(x => x.Id).Desc;

			if (maxcase > 0)
				subQuery.Where(x => x.Id < maxcase);

			if (type != null)
				subQuery.And(x => x.WorkItemType == type);

			subQuery.And(x => x.AssignedTo == user || x.CreatedBy == user);

			subQuery.Take(20);
			var query = session.QueryOver<WorkItem>()
				.WithSubquery.WhereProperty(x => x.Id).In(subQuery)
				.OrderBy(x => x.CreateDate).Desc;

			return query;
		}
	}

	public class ListMyOpenCasesWithUserAndProfiles : ListMyOpenCasesBaseQuery
	{
		public override IQueryOver<WorkItem, WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, User user)
		{
			var query = base.GetQuery(session, type, maxcase, user)
				.Fetch(x => x.CreatedBy).Eager
				.Fetch(x => x.CreatedBy.Profile).Eager
				.Fetch(x => x.AssignedTo).Eager
				.Fetch(x => x.AssignedTo.Profile).Eager;
			return query;
		}
	}

	public class ListMyOpenCasesWithSignOffs : ListMyOpenCasesBaseQuery
	{
		public override IQueryOver<WorkItem, WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, User user)
		{
			var query = base.GetQuery(session, type, maxcase, user)
				.Fetch(x => x.SignOffs).Eager
				.TransformUsing(Transformers.DistinctRootEntity);
			return query;
		}
	}

	public class ListMyOpenCases
	{
		public IEnumerable<WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, User user)
		{
			var basicWithUser = new ListMyOpenCasesWithUserAndProfiles().GetQuery(session, type, maxcase, user)
				.UnderlyingCriteria
				.SetFetchMode("System", FetchMode.Eager)
				.SetFetchMode("System.Area", FetchMode.Eager)
				.SetFetchMode("System.Category", FetchMode.Eager).Future<WorkItem>();
			var basicWithSignoffs = new ListMyOpenCasesWithSignOffs().GetQuery(session, type, maxcase, user).Future();
			return basicWithSignoffs;
		}
	}

}