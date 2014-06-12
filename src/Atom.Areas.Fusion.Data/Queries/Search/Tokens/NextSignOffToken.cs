using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class NextSignOffToken : BaseSearchToken, ISearchToken
	{
		public NextSignOffToken() : base("nso", 0, "nso", "") { }

		public string RegularExpression()
		{
			return string.Format(@"({0})?:(?<{0}>({1}))", TokenName(), string.Join("|", new SignOffTypeEnum().ToLowerDescriptionArray(@"\s?")));
		}

		public override string DisplayText(string[] values)
		{
			return " Requiring Next sign-off of: " + string.Join(",", values);
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			WorkItem workitem = null;
			WorkItemSignOff signoff = null;
			WorkItemSignOff signoff2 = null;

			searchCriteria.CreateDefaultDisjunction(TokenName());

			// Has the Required sign off with no value.
			var hasExactSignOffCriteria = DetachedCriteria<WorkItemSignOff>.Create(() => signoff)
				.SetProjection(LambdaProjection.Property<WorkItemSignOff>(x => x.Workitem))
				.Add<WorkItemSignOff>(x => x.Workitem.Id == workitem.Id)
				.Add(SqlExpression.IsNull<WorkItemSignOff>(x => x.SignedOff))
				.Add<WorkItemSignOff>(x => x.SignOffType == (SignOffTypeEnum)Enum.Parse(typeof(SignOffTypeEnum), value.ToString().Replace(" ", ""), true)); 

			// Less than the current sign off as completed
			var hasEarlierSignOffsCompleted = DetachedCriteria<WorkItemSignOff>.Create(() => signoff2)
				.SetProjection(LambdaProjection.Property<WorkItemSignOff>(x => x.Workitem))
				.Add<WorkItemSignOff>(x => x.Workitem.Id == workitem.Id)
				.Add(SqlExpression.IsNull<WorkItemSignOff>(x => x.SignedOff))
				.Add<WorkItemSignOff>(x => x.SignOffType < (SignOffTypeEnum)Enum.Parse(typeof(SignOffTypeEnum), value.ToString().Replace(" ", ""), true));

			var conjunction = Restrictions.Conjunction()
				.Add<WorkItem>(x => x.WorkStatus < WorkItemStatus.Closed)
				.Add(Subqueries.Exists(hasExactSignOffCriteria))
				.Add(Subqueries.NotExists(hasEarlierSignOffsCompleted));

			searchCriteria.Disjunctions[TokenName()].Add(conjunction);
				
			return searchCriteria;
		}
	}
}