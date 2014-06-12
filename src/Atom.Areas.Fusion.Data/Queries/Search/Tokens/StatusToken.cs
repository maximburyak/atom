using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class StatusToken : BaseSearchToken, ISearchToken
	{
		public StatusToken() : base("status", 1, "status", "e.g. open") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>({3}))", RegExShort(), RegExLong(), TokenName(), string.Join("|", new WorkItemStatus().ToLowerDescriptionArray(@"\s?")));
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" with status(es) of: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;

			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.WorkStatus == (WorkItemStatus)Enum.Parse(typeof(WorkItemStatus), _value.ToString().Replace(" ", ""), true));
			return searchCriteria;
		}
	}
}