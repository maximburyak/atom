using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListSingleUser : IQuery
	{
		public int id { get; set; }
		public string name { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(User));

			if (id > 0)
				criteria.Add<User>(x => x.Id == id);

			if (!string.IsNullOrEmpty(name))
				criteria.Add<User>(x => x.UserID == name);

			criteria.SetFetchMode("Profile", FetchMode.Eager)
				.SetFetchMode("Profile.Avatars", FetchMode.Eager);
				//.SetMaxResults(1);

			return criteria;
		}
	}
}
