using System.Linq;
using System.Security.Principal;
using Atom.Main.Data;
using Atom.Main.Domain.Models;
using BeValued.Utilities.MVC.Services;
using NHibernate;

namespace Atom.Main.Services
{
	public interface IAtomCacheManager
	{
		void RemoveCacheItems();
		IPrincipal GetUserIPrincipal(string name);
	}

	public class AtomCacheManager : IAtomCacheManager
	{
		private readonly ISession _session;
		private readonly CacheService _cacheService;

		public AtomCacheManager(ISession session)
		{
			_session = session;
			_cacheService = new AtomCacheService();
		}

		public void RemoveCacheItems()
		{
			_cacheService.RemoveAll();
		}

		public IPrincipal GetUserIPrincipal(string name)
		{
			var user = _cacheService.Get("user-" + name,
												  () => new ListSingleUser { name = name }.GetQuery(_session).List<AtomUser>().First(), 60);
			return user;
		}
	}

	public class AtomCacheService : CacheService
	{
		public override string CacheIndexKey
		{
			get { return "atom"; }
		}
	}
}
