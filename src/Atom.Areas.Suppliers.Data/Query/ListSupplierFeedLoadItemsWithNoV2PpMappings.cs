using Atom.Areas.Suppliers.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Suppliers.Data.Query
{
	public class ListSupplierFeedLoadItemsWithNoV2PpMappings
	{
		public string Category { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			PowerplaySuppliedFeedItem Item = null;
			Format format = null;
			FormatType formatType = null;
			ClassCms classCms = null;
			SupplierCategoryMappingV2Powerplay v2Mapping = null;

			// These are not used, but by Future<>, you avoid a Select N+1;
			var formattypes = session.QueryOver<FormatType>().Future();
			var formats = session.QueryOver<Format>().Future();
			var classes = session.QueryOver<ClassCms>().List();

			var criteria = DetachedCriteria<PowerplaySuppliedFeedItem>.Create(() => Item)
				.CreateAlias(() => Item.Format, () => format)
				.CreateAlias(() => Item.FormatType, () => formatType)
				.CreateAlias(() => Item.Class, () => classCms);
			//.SetFetchMode(() => format, FetchMode.Eager)
			//.SetFetchMode(() => formatType, FetchMode.Eager)
			//.SetFetchMode(() => classCms, FetchMode.Eager);

			var mappingCriteria = DetachedCriteria<SupplierCategoryMappingV2Powerplay>.Create(() => v2Mapping)
				.SetProjection(Projections.Property<SupplierCategoryMappingV2Powerplay>(x => x.Id))
				.Add<SupplierCategoryMappingV2Powerplay>(x => x.Format == format.FormatCode)
				.Add<SupplierCategoryMappingV2Powerplay>(x => x.FormatType == formatType.FormatTypeCode)
				.Add<SupplierCategoryMappingV2Powerplay>(x => x.Class == classCms.Class);

			if (!string.IsNullOrWhiteSpace(Category))
				mappingCriteria.Add<SupplierCategoryMappingV2Powerplay>(x => x.Category == Category);

			criteria.Add(Subqueries.NotExists(mappingCriteria));

			criteria.SetProjection(
				Projections.Distinct(Projections.ProjectionList()
										.Add(Projections.Property<PowerplaySuppliedFeedItem>(p => p.Format), "Format")
										.Add(Projections.Property<PowerplaySuppliedFeedItem>(p => p.FormatType), "FormatType")
										.Add(Projections.Property<PowerplaySuppliedFeedItem>(p => p.Class), "Class")

					));
			criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PowerplaySuppliedFeedItem)));
			return criteria.GetExecutableCriteria(session);
		}
	}
}