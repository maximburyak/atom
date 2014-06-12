using NHibernate;

namespace Atom.Areas.Fusion.Data.Queries
{
	public interface IQuery
	{
		ICriteria GetQuery(ISession session);
	}
}
