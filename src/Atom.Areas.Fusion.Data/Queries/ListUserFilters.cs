using System.Collections.Generic;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;
using NHibernate.SqlCommand;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListUserFilters : IQuery
	{
		public string UserID { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			SupportProfile profile = null;
			List<Filter> filters = null;
			Filter filter = null;
			var criteria = session.CreateCriteria(typeof (User))
				.Add<User>(u => u.UserID == UserID)
				.CreateAlias<User>(x => x.Profile, () => profile)
				.CreateAlias<User>(s => s.Profile.Filters, () => filters, JoinType.InnerJoin)
				.SetMaxResults(100);
			return criteria;
		}
	}
}
