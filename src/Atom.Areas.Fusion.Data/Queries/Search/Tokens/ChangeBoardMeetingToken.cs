using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class ChangeBoardMeetingToken : BaseSearchToken, ISearchToken
	{
		public ChangeBoardMeetingToken() : base("cabmeeting", 3, "cabmeeting", "") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>)", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" requiring change board sign-off");
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			WorkItem workitem = null;
			WorkItemSignOff signoff = null;

			searchCriteria.CreateDefaultDisjunction(TokenName());

			var signoffCriteria = DetachedCriteria<WorkItemSignOff>.Create(() => signoff)
				.SetProjection(LambdaProjection.Property<WorkItemSignOff>(x => x.Workitem))
				.Add<WorkItemSignOff>(x => x.Workitem.Id == workitem.Id)
				.Add<WorkItemSignOff>(x => x.SignOffType <= SignOffTypeEnum.ChangeBoardAcceptance)
				.Add(SqlExpression.IsNull<WorkItemSignOff>(x => x.SignedOff));
			searchCriteria.Criteria.Add<Crf>(x => x.CrfStatus == CrfStatus.Requested);

			
			searchCriteria.Disjunctions[TokenName()]
				.Add(Subqueries.Exists(signoffCriteria));

			return searchCriteria;
		}
	}
}