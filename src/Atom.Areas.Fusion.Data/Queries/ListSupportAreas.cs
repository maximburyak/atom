using Atom.Areas.Fusion.Domain.Models;
using NHibernate;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListSupportAreas : IQuery
	{
		public ICriteria GetQuery(ISession session)
		{
			return session.CreateCriteria(typeof(Area)); ;
		}
	}
}
