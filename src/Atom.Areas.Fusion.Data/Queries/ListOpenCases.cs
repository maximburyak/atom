using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Atom.Areas.Fusion.Data.Queries
{
	public abstract class ListOpenCasesBaseQuery
	{
		public virtual IQueryOver<WorkItem, WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, HandlingDepartment dept)
		{
			var subQuery = QueryOver.Of<WorkItem>()
				.And(x => x.WorkStatus < WorkItemStatus.Closed)
				.Select(x => x.Id)
				.OrderBy(x => x.CreateDate).Desc;

			if (maxcase > 0)
				subQuery.Where(x => x.Id < maxcase);

			if (type != null)
				subQuery.And(x => x.WorkItemType == type);

			if (dept != null)
				subQuery.And(x => x.Department.Id == dept.Id);

			subQuery.Take(20);
			var query = session.QueryOver<WorkItem>()
				.WithSubquery.WhereProperty(x => x.Id).In(subQuery)
				.OrderBy(x => x.CreateDate).Desc;

			return query;
		}
	}

	public class ListOpenCasesWithUserAndProfiles : ListOpenCasesBaseQuery
	{
		public override IQueryOver<WorkItem, WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, HandlingDepartment dept)
		{
			var query = base.GetQuery(session, type, maxcase, dept)
				.Fetch(x => x.CreatedBy).Eager
				.Fetch(x => x.CreatedBy.Profile).Eager
				.Fetch(x => x.AssignedTo).Eager
				.Fetch(x => x.AssignedTo.Profile).Eager;
			return query;
		}
	}

	public class ListOpenCasesWithSignOffs : ListOpenCasesBaseQuery
	{
		public override IQueryOver<WorkItem, WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, HandlingDepartment dept)
		{
			var query = base.GetQuery(session, type, maxcase, dept)
				.Fetch(x => x.SignOffs).Eager
				.TransformUsing(Transformers.DistinctRootEntity);
			return query;
		}
	}
	public class ListOpenCases
	{
		public IEnumerable<WorkItem> GetQuery(ISession session, WorkItemTypeEnum? type, int? maxcase, HandlingDepartment dept)
		{
			var basicWithUser = new ListOpenCasesWithUserAndProfiles().GetQuery(session, type, maxcase, dept)
				.UnderlyingCriteria
				.SetFetchMode("System", FetchMode.Eager)
				.SetFetchMode("System.Area", FetchMode.Eager)
				.SetFetchMode("System.Category", FetchMode.Eager).Future<WorkItem>();
			var basicWithSignoffs = new ListOpenCasesWithSignOffs().GetQuery(session, type, maxcase, dept).Future();
			return basicWithSignoffs;
		}
	}
}