using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class WorkItemTypeIndividualToken : BaseSearchToken, ISearchToken
	{
		private readonly WorkItemTypeEnum _type;
		public WorkItemTypeIndividualToken(WorkItemTypeEnum type)
			: base(type.GetDescription().ToLower(), 0, type.GetDescription().ToLower(), "1234")
		{
			_type = type;
		}

		public string RegularExpression()
		{
			return string.Format(@"{0}?:(?<{0}>\d+)", TokenName());
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" With Id's: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			searchCriteria.Criteria.Add<WorkItem>(x => x.WorkItemType == _type);
			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.Id == Int32.Parse(_value.ToString()));
			return searchCriteria;
		}
	}
}
