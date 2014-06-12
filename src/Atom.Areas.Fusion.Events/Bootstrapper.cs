using Atom.Main.Domain;
using StructureMap;

namespace Atom.Areas.Fusion.Events
{
	public class Bootstrapper : IAreaBootstrap
	{
		public void Bootstrap()
		{
			ObjectFactory.Configure(x => x.Scan(y =>
			{
				y.TheCallingAssembly();
				y.AddAllTypesOf(typeof(IEventHandler<>));
			}));
		}
	}
}
