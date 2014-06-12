using NHibernate;

namespace Atom.Areas.Suppliers.Data.Query
{
	public interface IQuery
	{
		ICriteria GetQuery(ISession session);
	}
}
