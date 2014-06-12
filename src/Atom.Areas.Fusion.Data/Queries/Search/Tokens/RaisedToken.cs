using Atom.Areas.Fusion.Domain.Models;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class RaisedToken : BaseSearchToken, ISearchToken
	{
		public RaisedToken() : base("raised", 1, "raised", "User Name") { }

		public string RegularExpression()
		{
            return string.Format(@"{0}(?:{1})?:(?<{2}>[a-z]+(?:\.[a-z]+)?)", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" Raised by: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value.Replace("\"", "");
			User CreatedBy = null;

			if (searchCriteria.Criteria.GetCriteriaByAlias(() => CreatedBy) == null)
				searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.CreatedBy, () => CreatedBy);

			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.CreatedBy.UserID == _value.ToString());

			return searchCriteria;
		}
	}
}