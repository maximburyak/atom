using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListClassCms : IQuery
	{
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(ClassCms));
			return criteria;
		}
	}
}
