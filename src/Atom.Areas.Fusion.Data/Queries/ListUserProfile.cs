using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListUserProfile : IQuery
	{
		public string UserID { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof (User))
				.Add<User>(u => u.UserID == UserID)
				.SetFetchMode("User.Profile.Avatars", FetchMode.Eager)
				.SetFetchMode("User.Profile.Signatures", FetchMode.Eager)
				.SetMaxResults(1);

			return criteria;
		}
	}
}