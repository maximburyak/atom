using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class AssignedRequireSignOffToken : BaseSearchToken, ISearchToken
	{
		public AssignedRequireSignOffToken() : base("sorequired", 3, "sorequired", "") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>)", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return " Items requiring signoff";
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			WorkItem workitem = null;
			WorkItemSignOff signoff = null;
			User AssignedTo = null;

			if (searchCriteria.Criteria.GetCriteriaByAlias(() => AssignedTo) == null)
				searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.AssignedTo, () => AssignedTo);

			searchCriteria.Criteria.Add<WorkItem>(x => x.AssignedTo.UserID == value.ToString());

			searchCriteria.CreateDefaultDisjunction(TokenName());

			// Create exists query for the SignOffs
			var signoffCriteria = DetachedCriteria<WorkItemSignOff>.Create(() => signoff)
				.SetProjection(LambdaProjection.Property<WorkItemSignOff>(x => x.Workitem))
				.Add<WorkItemSignOff>(x => x.Workitem.Id == workitem.Id)
				.Add(SqlExpression.IsNull<WorkItemSignOff>(x => x.SignedOff));

			searchCriteria.Disjunctions[TokenName()]
				.Add(Subqueries.Exists(signoffCriteria));

			return searchCriteria;
		}
	}
}