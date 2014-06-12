using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{

	public class ListSingleWorkItem : IQuery
	{
		public int Id { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(WorkItem));
			if (Id > 0)
				criteria.Add<WorkItem>(x => x.Id == Id);

            //IList<WorkItemChannel> channels = null;
            //Channel channel = null;
            //WorkItemSupplier supplier = null;
            //WorkItemProductGroup product = null;
            //WorkItemInsuranceCompany insco = null;
            //IList<WorkItemSupplier> suppliers = null;
            //IList<WorkItemProductGroup> productGroups = null;
            //IList<WorkItemInsuranceCompany> insuranceCompanies = null;
            //IList<Comment> comments = null;
            //IList<Document> documents = null;
            //IList<Subscription> subscriptions = null;

			criteria
                //.CreateAlias<WorkItem>(c => c.Channels, () => channels, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(ch => ch.Channels[0].Channel, () => channel, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(s => s.Suppliers, () => suppliers, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(sp => sp.Suppliers[0].Supplier, () => supplier, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(ic => ic.InsuranceCompanies, () => insuranceCompanies, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(i => i.InsuranceCompanies[0].InsuranceCompany, () => insco, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(pg => pg.ProductGroups, () => productGroups, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(p => p.ProductGroups[0].ProductGroup, () => product, JoinType.InnerJoin)
                //.CreateAlias<WorkItem>(co => co.Comments, () => comments, JoinType.InnerJoin)
				.SetMaxResults(1);

			return criteria;
		}
	}
}

