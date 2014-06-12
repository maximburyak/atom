using System;
using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Event;
using NHibernate.Event.Default;

namespace Atom.Areas.Fusion.Data.Listeners
{

	//http://stackoverflow.com/questions/5087888/ipreupdateeventlistener-and-dynamic-update-true
	[Serializable]
	public class FusionFlushEntityEventListener : DefaultFlushEntityEventListener
	{
		protected override void DirtyCheck(FlushEntityEvent e)
		{
			base.DirtyCheck(e);
			if (e.DirtyProperties != null &&
				e.DirtyProperties.Any() &&
				e.Entity is AuditableFusionEntity)
				e.DirtyProperties = e.DirtyProperties
				 .Concat(GetAdditionalDirtyProperties(e)).ToArray();
		}

		static IEnumerable<int> GetAdditionalDirtyProperties(FlushEntityEvent @event)
		{
			yield return Array.IndexOf(@event.EntityEntry.Persister.PropertyNames, "AlteredBy");
			yield return Array.IndexOf(@event.EntityEntry.Persister.PropertyNames, "AlteredDate");
			//You can add any additional properties here.
		}
	}
}
