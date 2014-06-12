using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListSingleCrf : IQuery
	{
		public int id { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(Crf));
			criteria.SetMaxResults(1);
			if (id > 0)
				criteria.Add<Crf>(x => x.Id == id);

			return criteria;
		}
	}
}

