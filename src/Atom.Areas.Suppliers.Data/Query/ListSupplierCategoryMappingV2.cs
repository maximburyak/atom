using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListSupplierCategoryMappingV2 : IQuery
	{
		public string Category { get; set; }
		public int SupplierId { get; set; }
		public string Level1 { get; set; }
		public string Level2 { get; set; }
		public string Level3 { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(SupplierCategoryMappingV2));

			if (SupplierId > 0)
				criteria.Add<SupplierCategoryMappingV2>(x => x.SupplierID == SupplierId);

			if (!string.IsNullOrEmpty(Level1))
				criteria.Add<SupplierCategoryMappingV2>(x => x.Level1 == Level1);

			if (!string.IsNullOrEmpty(Level2))
				criteria.Add<SupplierCategoryMappingV2>(x => x.Level2 == Level2);

			if (!string.IsNullOrEmpty(Level3))
				criteria.Add<SupplierCategoryMappingV2>(x => x.Level3 == Level3);

			if (!string.IsNullOrEmpty(Category))
				criteria.Add<SupplierCategoryMappingV2>(x => x.Category == Category);

			return criteria;
		}
	}
}
