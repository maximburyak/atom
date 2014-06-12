using System;
using System.Threading;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace Atom.Areas.Fusion.Data.Listeners
{
	//http://ayende.com/blog/3987/nhibernate-ipreupdateeventlistener-ipreinserteventlistener

	[Serializable]
	public class FusionAuditEventListener : IPreUpdateEventListener, IPreInsertEventListener
	{
		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			var entity = @event.Entity as AuditableFusionEntity;
			if (entity != null)
			{
				var atomPrincipal = ((AtomPrincipal)Thread.CurrentPrincipal);
				entity.AlteredBy = atomPrincipal.User;
				entity.AlteredDate = atomPrincipal.Date;
				Set(@event.Persister, @event.State, "AlteredBy", entity.AlteredBy);
				Set(@event.Persister, @event.State, "AlteredDate", entity.AlteredDate);
			}
			return false;
		}

		public bool OnPreInsert(PreInsertEvent @event)
		{
			var entity = @event.Entity as AuditableFusionEntity;
			if (entity != null && !entity.CreateDate.HasValue)
			{
				var atomPrincipal = ((AtomPrincipal)Thread.CurrentPrincipal);
				entity.CreateDate = atomPrincipal.Date;
				entity.CreatedBy = atomPrincipal.User;
				Set(@event.Persister, @event.State, "CreateDate", entity.CreateDate);
				Set(@event.Persister, @event.State, "CreatedBy", entity.CreatedBy);
			}
			return false;
		}

		private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
		{
			var index = Array.IndexOf(persister.PropertyNames, propertyName);
			if (index == -1)
				return;
			state[index] = value;
		}
	}
}