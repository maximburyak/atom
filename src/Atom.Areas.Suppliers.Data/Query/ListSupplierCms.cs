using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListSupplierCms : IQuery
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(SupplierCms));

			if (Id > 0)
				criteria.Add<SupplierCms>(x => x.Id == Id);

			if (!string.IsNullOrEmpty(Name))
				criteria.Add<SupplierCms>(x => x.Name == Name);

			return criteria;
		}
	}
}
