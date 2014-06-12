using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{

	public class ListCategory : IQuery
	{
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(Category))
				.Add<Category>(x => x.Enabled)
				.SetMaxResults(50);
			return criteria;
		}
	}
}
