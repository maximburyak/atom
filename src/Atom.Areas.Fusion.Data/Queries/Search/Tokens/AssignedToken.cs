using Atom.Areas.Fusion.Domain.Models;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class AssignedToken : BaseSearchToken, ISearchToken
	{
		public AssignedToken() : base("assigned", 1, "assigned", "User Name") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>[a-z]+)", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" Assigned to \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value.Replace("\"", "");
			User AssignedTo = null;
			if (searchCriteria.Criteria.GetCriteriaByAlias(() => AssignedTo) == null)
				searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.AssignedTo, () => AssignedTo);

			searchCriteria.CreateDefaultDisjunction(TokenName());

			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.AssignedTo.UserID == _value.ToString());

			return searchCriteria;
		}
	}
}