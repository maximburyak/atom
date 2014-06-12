using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListEtlFilesProcessLogEntries : IQuery
	{
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(EtlFilesProcessLogEntry));
			return criteria;
		}
	}
}
