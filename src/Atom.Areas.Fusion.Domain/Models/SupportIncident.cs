using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class SupportIncident : WorkItem
	{
		[Required(ErrorMessage = "Location is a required field")]
		public virtual Location Location { get; set; }

		[Required(ErrorMessage = "Details is a required field")]
		public virtual Details System { get; set; }
		public virtual SupportIncidentStatus IncidentStatus { get; set; }

		public virtual ClosureReason ClosureReason { get; set; }

		public virtual IList<AdditionalInfo> AdditionalInfo { get; set; }

		public override void PutOnHold(User user)
		{
			if (!(IncidentStatus < SupportIncidentStatus.OnHold)) return;
			IncidentStatus = SupportIncidentStatus.OnHold;
			WorkStatus = WorkItemStatus.OnHold;
			base.PutOnHold(user);
		}

		public override void TakeOffHold(User user)
		{
			if (IncidentStatus != SupportIncidentStatus.OnHold) return;
			IncidentStatus = SupportIncidentStatus.InProgress;
			WorkStatus = WorkItemStatus.InProgress;
			base.TakeOffHold(user);
		}

		public virtual void CloseCase(User closedBy)
		{
			if (AssignedTo == null)
				AssignedTo = closedBy;

			IncidentStatus = SupportIncidentStatus.Closed;
			Close(closedBy);
		}

		public override void AssignTo(User assignTo, User assignedBy)
		{
			IncidentStatus = SupportIncidentStatus.InProgress;
			base.AssignTo(assignTo, assignedBy);
		}
	}
}