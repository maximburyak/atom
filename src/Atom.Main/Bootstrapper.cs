using Atom.Main.Registries;
using BeValued.Data.SQL;
using BeValued.Security;
using BeValued.Utilities;
using StructureMap;

namespace Atom.Main.Setup
{
	public class Bootstrapper : IBootstrapper
	{
		private static bool _hasStarted;

		public void BootstrapStructureMap()
		{
			ObjectFactory.Initialize(init =>
			{
				init.AddRegistry<SqlRegistry>();
				init.AddRegistry<MvcRegistry>();
				init.AddRegistry<NHibernateRegistry>();
				init.AddRegistry<UtilitiesRegistry>();
                init.AddRegistry<SecurityRegistry>();
			});
			log4net.Config.XmlConfigurator.Configure();
		}

		public static void Restart()
		{
			if (_hasStarted)
			{
				ObjectFactory.ResetDefaults();
			}
			else
			{
				Bootstrap();
				_hasStarted = true;
			}
		}

		public static void Bootstrap()
		{
			new Bootstrapper().BootstrapStructureMap();
		}
	}
}


