using System.Collections.Generic;
using Atom.Areas.Fusion.Data.Listeners;
using Atom.Main.Setup;
using BeValued.Data.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using StructureMap.Configuration.DSL;

namespace Atom.Main.Registries
{
	public class NHibernateRegistry : Registry
	{
		public NHibernateRegistry()
		{
			var listeners = new Dictionary<ListenerType, object>{
				                          		        {ListenerType.PreInsert, new FusionAuditEventListener()},
				                          		        {ListenerType.PreUpdate, new FusionAuditEventListener()},
				                          		        {ListenerType.FlushEntity, new FusionFlushEntityEventListener()}
								};

			var mappingAssemblies = new[]
			                         	{
			                         		NHUtility.AssemblyDirectory +"\\Atom.Main.Domain.dll", 
											NHUtility.AssemblyDirectory +"\\Atom.Areas.Suppliers.Data.dll",
											NHUtility.AssemblyDirectory +"\\Atom.Areas.Fusion.Data.dll",
			                         	};
			var configFile = NHUtility.AssemblyDirectory + "\\NHConfig.config";
			var builder = new NHibernateConfigurationBuilder(configFile: configFile, mappingAssemblies: mappingAssemblies, persistenceModel: ApConfig.Init(), listeners: listeners);
			var config = builder.Build();
			var sessionFactory = config.BuildSessionFactory();
			For<Configuration>().Singleton().Use(config);
			For<ISessionFactory>().Singleton().Use(sessionFactory);
			For<ISession>().HybridHttpOrThreadLocalScoped().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
			For(typeof(INHibernateRepository<>)).Use(typeof(NHibernateRepository<>));
		}
	}
}