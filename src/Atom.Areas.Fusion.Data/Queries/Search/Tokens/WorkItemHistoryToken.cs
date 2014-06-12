using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
using NHibernate.SqlCommand;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
    class WorkItemHistoryToken : BaseSearchToken, ISearchToken
    {
        public WorkItemHistoryToken() : base("viewed", 1, "viewed", "User Name") { }

        public string RegularExpression()
        {
            return string.Format(@"{0}(?:{1})?:(?<{2}>[a-z]+)", RegExShort(), RegExLong(), TokenName());
        }

        public override string DisplayText(string[] values)
        {
            return string.Format(" Recently viewed by: \"{0}\"", string.Join(",", values));
        }

        public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
        {
            _value = value.Replace("\"", "");
            User CreatedBy = null;
            IList<WorkItemHistory> History = null;

            if (searchCriteria.Criteria.GetCriteriaByAlias(() => History) == null)
                searchCriteria.Criteria.CreateAlias<WorkItem>(x => x.History, () => History, JoinType.InnerJoin)
                    .CreateAlias<WorkItem>(h => h.History[0].CreatedBy, () => CreatedBy, JoinType.InnerJoin)
                    .AddOrder<WorkItem>(h => h.History[0].CreateDate, Order.Desc);

            searchCriteria.CreateDefaultDisjunction(TokenName());
            searchCriteria.Disjunctions[TokenName()]
                .Add<WorkItemHistory>(x => x.CreatedBy.UserID == _value.ToString());

            return searchCriteria;
        }
    }
}
