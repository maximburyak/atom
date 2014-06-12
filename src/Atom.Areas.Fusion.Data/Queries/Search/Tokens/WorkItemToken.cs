using System;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class WorkItemToken : BaseSearchToken, ISearchToken
	{
		public WorkItemToken() : base("item", 1, "item", "1234") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>\d+)", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" With Id's: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value.Replace("\"", "");
			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.Id == Int32.Parse(_value.ToString()));

			return searchCriteria;
		}
	}
}