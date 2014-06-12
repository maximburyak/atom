using System;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class WorkItemDigitToken : BaseSearchToken, ISearchToken
	{
		public WorkItemDigitToken() : base("digit", 0, "digit", "1234") { }

		public string RegularExpression()
		{
			return string.Format(@"^(?<{0}>\d+)$", TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" With Id's of \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()].Add<WorkItem>(x => x.Id == Int32.Parse(_value.ToString()));
			return searchCriteria;
		}
	}
}