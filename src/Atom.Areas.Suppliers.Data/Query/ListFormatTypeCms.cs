using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListFormatTypeCms : IQuery
	{
		public ICriteria GetQuery(ISession session)
		{
		    var criteria = session.CreateCriteria(typeof (FormatType));
			return criteria;
		}
	}
}
