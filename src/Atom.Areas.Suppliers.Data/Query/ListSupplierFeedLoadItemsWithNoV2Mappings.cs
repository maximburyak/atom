using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListSupplierFeedLoadItemsWithNoV2Mappings
	{
		public int? SupplierId { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			SupplierFeedsLoadItem Item = null;
			SupplierCategoryMappingV2 v2Mapping = null;



			var criteria = DetachedCriteria<SupplierFeedsLoadItem>.Create(() => Item);
			if (SupplierId.HasValue)
				criteria.Add<SupplierFeedsLoadItem>(x => x.SupplierId == SupplierId.Value);

			var mappingCriteria = DetachedCriteria<SupplierCategoryMappingV2>.Create(() => v2Mapping)

				.SetProjection(Projections.Property<SupplierCategoryMappingV2>(x => x.SupplierID))
				.Add<SupplierCategoryMappingV2>(x => x.SupplierID == Item.SupplierId)
				.Add<SupplierCategoryMappingV2>(x => x.Level1 == Item.Level1)
				.Add<SupplierCategoryMappingV2>(x => x.Level2 == Item.Level2)
				.Add<SupplierCategoryMappingV2>(x => x.Level3 == Item.Level3);

			criteria.Add(Subqueries.NotExists(mappingCriteria));

			criteria.SetProjection(
				Projections.Distinct(Projections.ProjectionList()
										.Add(Projections.Property<SupplierFeedsLoadItem>(x => x.SupplierId), "SupplierId")
										.Add(Projections.Property<SupplierFeedsLoadItem>(p => p.Level1), "Level1")
										.Add(Projections.Property<SupplierFeedsLoadItem>(p => p.Level2), "Level2")
										.Add(Projections.Property<SupplierFeedsLoadItem>(p => p.Level3), "Level3")
										.Add(Projections.Property<SupplierFeedsLoadItem>(p => p.Level4), "Level4")

					));
			criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(SupplierFeedsLoadItem)));
			//var criteria = QueryOver.Of(() => Item);

			//if (SupplierId.HasValue)
			//    criteria.Where(x => x.SupplierId == SupplierId);

			//var mappingCriteria = QueryOver.Of(() => v2Mapping)
			//    .Where(x => x.SupplierID == Item.SupplierId)
			//    .And(x => x.Level1 == Item.Level1)
			//    .And(x => x.Level2 == Item.Level2)
			//    .And(x => x.Level3 == Item.Level3)
			//    .Select(x => x.SupplierID);

			//criteria.WithSubquery.WhereNotExists(mappingCriteria);
			//criteria.Select(
			//    Projections.Distinct(
			//        Projections.ProjectionList()
			//        .Add(Projections.Property<SupplierFeedsLoadItem>(x => x.SupplierId).WithAlias(() => Item.SupplierId))
			//        .Add(Projections.Property<SupplierFeedsLoadItem>(x => x.Level1).WithAlias(() => Item.Level1))
			//        .Add(Projections.Property<SupplierFeedsLoadItem>(x => x.Level2).WithAlias(() => Item.Level2))
			//        .Add(Projections.Property<SupplierFeedsLoadItem>(x => x.Level3).WithAlias(() => Item.Level3))
			//        .Add(Projections.Property<SupplierFeedsLoadItem>(x => x.Level4).WithAlias(() => Item.Level4))
			//    )
			//);
			//criteria.TransformUsing(Transformers.AliasToBean<SupplierFeedsLoadItem>());

			return criteria.GetExecutableCriteria(session);
			//return criteria.UnderlyingCriteria;
		}
	}
}