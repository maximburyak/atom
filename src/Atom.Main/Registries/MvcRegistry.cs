using Atom.Areas.Fusion.Events;
using Atom.Main.Domain;
using Atom.Main.Services;
using BeValued.Domain.Common.Services;
using NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Atom.Main.Registries
{
	public class MvcRegistry : Registry
	{
		public MvcRegistry()
		{
			Scan(x =>
			{
				x.AssembliesFromApplicationBaseDirectory();
				x.WithDefaultConventions();
				x.AddAllTypesOf(typeof(IEventHandler<>));
				x.AddAllTypesOf(typeof(IAreaBootstrap));
			});
			For<IAtomCacheManager>().Use<AtomCacheManager>();
			foreach (var bootstrap in ObjectFactory.GetAllInstances<IAreaBootstrap>())
			{
				bootstrap.Bootstrap();
			}

			//needed for "setter" injecting dependancies into Filters
			SetAllProperties(c =>
			{
				c.OfType<IDateService>();
				c.OfType<ISession>();
			});
		}
	}
}