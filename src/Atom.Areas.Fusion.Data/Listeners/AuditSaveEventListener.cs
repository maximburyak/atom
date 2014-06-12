using System;
using System.Threading;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Domain.Models;
using NHibernate.Event;
using NHibernate.Event.Default;

namespace Atom.Areas.Fusion.Data.Listeners
{
	[Serializable]
	public class AuditSaveEventListener : DefaultSaveEventListener
	{
		protected override object PerformSaveOrUpdate(SaveOrUpdateEvent evt)
		{

			var entity = evt.Entity as IAuditableFusionEntity;

			if (entity != null)
			{
				ProcessEntity(entity);
			}
			return base.PerformSaveOrUpdate(evt);
		}

		protected internal virtual void ProcessEntity(IAuditableFusionEntity entity)
		{
			var atomUser = ((IAtomUser)Thread.CurrentPrincipal);
			var user = new User { Id = atomUser.Id, EmailAddress = atomUser.EmailAddress, Name = atomUser.Name };

			entity.CreatedBy = entity.CreatedBy ?? user;
			entity.CreateDate = (!entity.CreateDate.HasValue) ? DateTime.Now : entity.CreateDate;
		}
	}


}