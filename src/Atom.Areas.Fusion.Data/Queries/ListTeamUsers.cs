using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;
using NHibernate.SqlCommand;

namespace Atom.Areas.Fusion.Data.Queries
{

	public class ListTeamUsers : IQuery
	{
		public string Team { get; set; }
		public string dept { get; set; }
		public bool? isAutoAssignedTo { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			SupportProfile Profile = null;
			HandlingDepartment Department = null;

			var criteria = session.CreateCriteria(typeof(User))
				.Add<User>(x => x.AccessLevel > 0);

			if (Team != null)
				criteria.Add<User>(x => x.Team == Team);

			if (isAutoAssignedTo.HasValue)
			{
				if (criteria.GetCriteriaByAlias(() => Profile) == null)
					criteria.CreateAlias<User>(u => u.Profile, () => Profile, JoinType.InnerJoin)
						.SetFetchMode<User>(x => x.Profile, FetchMode.Eager);

				criteria.Add<User>(x => x.Profile.IsAssignedToAuto == isAutoAssignedTo.Value);
			}

			if (!string.IsNullOrEmpty(dept))
				criteria.CreateAlias<User>(u => u.Department, () => Department)
					.Add<User>(x => x.Department.Description == dept);

			return criteria;
		}
	}
}