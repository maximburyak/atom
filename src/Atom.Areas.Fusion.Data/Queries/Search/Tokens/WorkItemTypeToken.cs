using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.LambdaExtensions;
namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class WorkItemTypeToken : BaseSearchToken, ISearchToken
	{
		public WorkItemTypeToken() : base("worktype", 1, "worktype", "e.g. incident") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>({3}))", RegExShort(), RegExLong(), TokenName(), string.Join("|", new WorkItemTypeEnum().ToLowerDescriptionArray()));
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" of type's: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(
				x => x.WorkItemType == (WorkItemTypeEnum)Enum.Parse(typeof(WorkItemTypeEnum), _value.ToString().Replace(" ", ""), true));

			return searchCriteria;
		}
	}
}