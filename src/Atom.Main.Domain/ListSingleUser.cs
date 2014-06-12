using Atom.Main.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Main.Data
{
	public class ListSingleUser
	{
		public int id { get; set; }
		public string name { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(AtomUser));

			if (id > 0)
				criteria.Add<AtomUser>(x => x.Id == id);

			if (!string.IsNullOrEmpty(name))
				criteria.Add<AtomUser>(x => x.UserID == name);

			return criteria;
		}
	}
}
