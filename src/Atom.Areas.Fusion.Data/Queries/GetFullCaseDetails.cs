using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class GetFullCaseDetails
	{
		public int CaseId { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(SupportIncident))
				.Add<User>(u => u.Id == CaseId)
				.SetFetchMode("System",FetchMode.Eager)
				.SetFetchMode("System.Area",FetchMode.Eager)
				.SetFetchMode("System.Category", FetchMode.Eager)
				.SetFetchMode("CreatedBy", FetchMode.Eager)
				.SetFetchMode("AssignedTo", FetchMode.Eager)
				.SetFetchMode("AssignedTo.Profile", FetchMode.Eager)
				.SetFetchMode("CreatedBy.Profile", FetchMode.Eager)
				.SetFetchMode("Comments", FetchMode.Eager)
				.SetFetchMode("Comments.CreatedBy", FetchMode.Eager)
				.SetFetchMode("Comments.CreatedBy.Profile", FetchMode.Eager);
			return criteria;
		}
	}
}