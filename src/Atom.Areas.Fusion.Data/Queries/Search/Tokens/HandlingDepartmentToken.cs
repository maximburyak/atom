using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class HandlingDepartmentToken : BaseSearchToken, ISearchToken
	{
		public HandlingDepartmentToken() : base("handlingdepartment", 1, "handlingdepartment", "Dept") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>({3}))", RegExShort(), RegExLong(), TokenName(), string.Join("|", new HandlingDepartmentTypeEnum().ToLowerDescriptionArray()));
		}

		public override string DisplayText(string[] values)
		{
			return string.Format("  Handled by departments: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			HandlingDepartment Department = null;
			if (searchCriteria.Criteria.GetCriteriaByAlias(() => Department) == null)
				searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.Department, () => Department);

			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.Department.Description == _value.ToString());

			return searchCriteria;
		}
	}
}