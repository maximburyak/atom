using Atom.Areas.Fusion.Domain.Models;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class LastUpdatedByToken : BaseSearchToken, ISearchToken
	{
		public LastUpdatedByToken() : base("lastupdated", 1, "lastupdated", "User Name") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>[a-z]+)", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" Last updated by \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			User AlteredBy = null;

			if (searchCriteria.Criteria.GetCriteriaByAlias(() => AlteredBy) == null)
				searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.AlteredBy, () => AlteredBy);
			
			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.AlteredBy.UserID == _value.ToString())
				.Add<WorkItem>(x => x.WorkStatus == WorkItemStatus.Open);
			return searchCriteria;
		}
	}
}