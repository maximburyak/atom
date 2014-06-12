using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class LastUpdatedByNameToken : BaseSearchToken, ISearchToken
	{
		public LastUpdatedByNameToken() : base("lastupdatedby", 1, "lastupdatedby", "User Name") { }


		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>""[a-z]+\s?[a-z]+?"")", RegExShort(), RegExLong(), TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" Last updated by \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			User AlteredBy = null;

			searchCriteria.CreateDefaultDisjunction(TokenName());

			searchCriteria.Disjunctions[TokenName()]
				.Add(SqlExpression.Like<WorkItem>(x => x.AlteredBy.Name, _value.ToString(), MatchMode.Start))
				.Add<WorkItem>(x => x.WorkStatus == WorkItemStatus.Open);
			return searchCriteria;
		}
	}
}