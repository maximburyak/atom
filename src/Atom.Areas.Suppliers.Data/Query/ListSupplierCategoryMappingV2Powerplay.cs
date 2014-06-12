using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListSupplierCategoryMappingV2Powerplay : IQuery
	{
		public int Id { get; set; }
		public string Category { get; set; }
		public string Format { get; set; }
		public string FormatType { get; set; }
		public string Class { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(SupplierCategoryMappingV2Powerplay));

			if (Id > 0)
				criteria.Add<SupplierCategoryMappingV2Powerplay>(x => x.Id == Id);

			if (!string.IsNullOrEmpty(Category))
				criteria.Add<SupplierCategoryMappingV2Powerplay>(x => x.Category == Category);

			if (!string.IsNullOrEmpty(Format))
				criteria.Add<SupplierCategoryMappingV2Powerplay>(x => x.Format == Format);

			if (!string.IsNullOrEmpty(FormatType))
				criteria.Add<SupplierCategoryMappingV2Powerplay>(x => x.FormatType == FormatType);

			if (!string.IsNullOrEmpty(Class))
				criteria.Add<SupplierCategoryMappingV2Powerplay>(x => x.Class == Class);

			return criteria;
		}
	}
}
