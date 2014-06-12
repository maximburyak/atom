using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class RaisedNameToken : BaseSearchToken, ISearchToken
	{
		public RaisedNameToken() : base("raised", 1, "raisedname", "User Name") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>""[a-z]+\s?[a-z]+?"")", RegExShort(), RegExLong(), TokenName());
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
				.Add(SqlExpression.Like<WorkItem>(x => x.CreatedBy.Name, _value.ToString(), MatchMode.Start));

			return searchCriteria;
		}
	}
}