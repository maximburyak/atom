using System;
using System.Linq;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class Project : WorkItem
	{
		public virtual ProjectStatus Status { get; set; }

		public virtual void UpdateSignOff(WorkItemSignOff signOff, User user)
		{
			var signOffToUpdate = SignOffs.FirstOrDefault(x => x.Id == signOff.Id);
			if (signOffToUpdate.Id <= 0 || signOffToUpdate.SignedOff != null)
				return;

			signOffToUpdate.SignedOffBy = user;
			signOffToUpdate.SignedOff = DateTime.Now;
			if (WorkStatus == WorkItemStatus.AwaitingApproval && signOff.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance)
			{
				WorkStatus = WorkItemStatus.Open;
				Status = ProjectStatus.Approved;
			}

			if (CanClose())
				CloseProject(user);

		}

		public override void AssignTo(User AssignTo, User AssignedBy)
		{
			Status = ProjectStatus.InProgress;
			WorkStatus = WorkItemStatus.InProgress;
			base.AssignTo(AssignTo, AssignedBy);
		}

		public override void PutOnHold(User user)
		{
			if (Status != ProjectStatus.InProgress) return;
			Status = ProjectStatus.OnHold;
			WorkStatus = WorkItemStatus.OnHold;
			base.PutOnHold(user);
		}

		public override void TakeOffHold(User user)
		{
			if (Status != ProjectStatus.OnHold) return;
			Status = ProjectStatus.InProgress;
			WorkStatus = WorkItemStatus.InProgress;
			base.TakeOffHold(user);
		}

		public virtual void Reject(User user)
		{
			if (Status == ProjectStatus.Rejected) return;

			Status = ProjectStatus.Rejected;
			WorkStatus = WorkItemStatus.Closed;
			ClosedBy = user;
			Rejected = DateTime.Now;
			ClosedDate = DateTime.Now;
		}



		private void CloseProject(User user)
		{
			if (Status == ProjectStatus.Completed)
				return;

			Status = ProjectStatus.Completed;
			Close(user);
		}

		public virtual void CompleteProject(User user, Project project)
		{
			if (Status == ProjectStatus.Completed) return;

			EstimatedUnitsOfWork = project.EstimatedUnitsOfWork;
			ActualUnitsOfWork = project.ActualUnitsOfWork;
			EstimatedStartDate = project.EstimatedStartDate;
			CompletionComment = project.CompletionComment;
			if (CanClose())
				CloseProject(user);
		}
	}
}
