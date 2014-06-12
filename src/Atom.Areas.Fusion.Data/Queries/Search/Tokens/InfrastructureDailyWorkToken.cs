using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class InfrastructureDailyWorkToken : BaseSearchToken, ISearchToken
	{
		public InfrastructureDailyWorkToken() : base("dailywork", 0, "dailywork", "Dept") { }

		public string RegularExpression()
		{
			return string.Format(@"({0})?:(?<{0}>({1}))", TokenName(), string.Join("|", new HandlingDepartmentTypeEnum().ToLowerDescriptionArray()));
		}

		public override string DisplayText(string[] values)
		{
			return string.Format("  Daily Work by departments: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			HandlingDepartment Department = null;
			if (searchCriteria.Criteria.GetCriteriaByAlias(() => Department) == null)
				searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.Department, () => Department);

			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.Department.Description == _value.ToString())
				.Add(Restrictions.Or(
						Restrictions.Or(
							SqlExpression.CriterionFor<WorkItem>(x => x.CreateDate > DateTime.Now.AddDays(-1)),
							SqlExpression.CriterionFor<WorkItem>(x => x.ClosedDate > DateTime.Now.AddDays(-1))
							),
						SqlExpression.CriterionFor<WorkItem>(x => x.WorkStatus < WorkItemStatus.Closed))
				);
			return searchCriteria;
		}
	}
}