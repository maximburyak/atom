using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class WorkItemTextToken : BaseSearchToken, ISearchToken
	{
		public WorkItemTextToken() : base("text", 0, "text", "Text") { }

		public string RegularExpression()
		{
			return string.Format(@"(?<{0}>.+)", TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" Items with text like: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add(SqlExpression.Like<WorkItem>(x => x.Summary, _value.ToString(), MatchMode.Anywhere));

			return searchCriteria;
		}
	}
}