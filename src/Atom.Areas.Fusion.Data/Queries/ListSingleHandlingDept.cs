using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListSingleHandlingDept : IQuery
	{
		public int? id { get; set; }
		public string description { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(HandlingDepartment));
			criteria.SetMaxResults(1);
			if (id.HasValue)
				criteria.Add<HandlingDepartment>(x => x.Id == id);

			if (!string.IsNullOrEmpty(description))
				criteria.Add<HandlingDepartment>(x => x.Description == description);

			return criteria;
		}
	}
}
