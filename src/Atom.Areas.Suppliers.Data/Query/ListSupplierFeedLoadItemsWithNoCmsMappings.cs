using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListSupplierFeedLoadItemsWithNoCmsMappings : IQuery
	{
		public int? Id { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			SupplierFeedsLoadItem Item = null;
			SupplierCategoryMappingCms CmsMapping = null;
			var criteria = DetachedCriteria<SupplierFeedsLoadItem>.Create(() => Item);

			if (Id.HasValue)
				criteria.Add<SupplierFeedsLoadItem>(x => x.SupplierId == Id.Value);

			var mappingCriteria = DetachedCriteria<SupplierCategoryMappingCms>.Create(() => CmsMapping)
				.SetProjection(LambdaProjection.Property<SupplierCategoryMappingCms>(x => x.SupplierID))
				.Add<SupplierCategoryMappingCms>(x => x.SupplierID == Item.SupplierId)
				.Add<SupplierCategoryMappingCms>(x => x.Level1 == Item.Level1)
				.Add<SupplierCategoryMappingCms>(x => x.Level2 == Item.Level2)
				.Add<SupplierCategoryMappingCms>(x => x.Level3 == Item.Level3)
				.Add<SupplierCategoryMappingCms>(x => x.Level4 == Item.Level4);

			criteria.Add(Subqueries.NotExists(mappingCriteria));


			criteria.SetProjection(
				Projections.Distinct(Projections.ProjectionList()
					.Add(LambdaProjection.Property<SupplierFeedsLoadItem>(p => p.SupplierId), "SupplierId")
					.Add(LambdaProjection.Property<SupplierFeedsLoadItem>(p => p.Level1), "Level1")
					.Add(LambdaProjection.Property<SupplierFeedsLoadItem>(p => p.Level2), "Level2")
					.Add(LambdaProjection.Property<SupplierFeedsLoadItem>(p => p.Level3), "Level3")
					.Add(LambdaProjection.Property<SupplierFeedsLoadItem>(p => p.Level4), "Level4")

					));
			criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(SupplierFeedsLoadItem)));
			return criteria.GetExecutableCriteria(session);
		}
	}
}
