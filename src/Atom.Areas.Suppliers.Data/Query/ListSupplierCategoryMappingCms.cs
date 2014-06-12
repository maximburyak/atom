using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{

	public class ListSupplierCategoryMappingCms : IQuery
	{
		public int SupplierId { get; set; }
		public string Level1 { get; set; }
		public string Level2 { get; set; }
		public string Level3 { get; set; }
		public string Level4 { get; set; }
		public string Format { get; set; }
		public string FormatType { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(SupplierCategoryMappingCms));

			if (SupplierId > 0)
				criteria.Add<SupplierCategoryMappingCms>(x => x.SupplierID == SupplierId);

			if (!string.IsNullOrEmpty(Level1))
				criteria.Add<SupplierCategoryMappingCms>(x => x.Level1 == Level1);

			if (!string.IsNullOrEmpty(Level2))
				criteria.Add<SupplierCategoryMappingCms>(x => x.Level2 == Level2);

			if (!string.IsNullOrEmpty(Level3))
				criteria.Add<SupplierCategoryMappingCms>(x => x.Level3 == Level3);

			if (!string.IsNullOrEmpty(Level4))
				criteria.Add<SupplierCategoryMappingCms>(x => x.Level4 == Level4);

			if (!string.IsNullOrEmpty(Format))
				criteria.Add<SupplierCategoryMappingCms>(x => x.Format == Format);

			if (!string.IsNullOrEmpty(FormatType))
				criteria.Add<SupplierCategoryMappingCms>(x => x.FormatType == FormatType);

			return criteria;
		}
	}
}
