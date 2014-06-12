using Atom.Areas.Fusion.Domain.Models;
using BeValued.Data.NHibernate;
using NHibernate;

namespace Atom.Areas.Fusion.Events.WorkItem
{
	public class WorkItemHistoryEvent : IDomainEvent
	{
		public Domain.Models.WorkItem WorkItem { get; set; }
	}

	public class WorkItemHistoryHandler : EventHandler, IEventHandler<WorkItemHistoryEvent>
	{
		private readonly ISession _session;

		public WorkItemHistoryHandler(ISession session)
		{
			_session = session;
		}

		public void Handle(WorkItemHistoryEvent args)
		{
			var wiRepo = new NHibernateRepository<Domain.Models.WorkItem>(_session);
			var hiRepo = new NHibernateRepository<WorkItemHistory>(_session);
			var hi = new WorkItemHistory();

			hiRepo.Save(hi);
			args.WorkItem.AddHistory(hi);
			wiRepo.Save(args.WorkItem);
		}
	}
}
