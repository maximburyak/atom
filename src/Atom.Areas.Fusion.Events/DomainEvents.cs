namespace Atom.Areas.Fusion.Events
{
	public static class DomainEvents
	{
		public static void Raise<T>(T args) where T : IDomainEvent
		{
			foreach (IEventHandler<T> handler in StructureMap.ObjectFactory.GetAllInstances(typeof(IEventHandler<T>)))
				handler.Handle(args);
		}
	}

	public interface IEventHandler<T>
	{
		void Handle(T args);
	}

	public interface IDomainEvent{}
}
