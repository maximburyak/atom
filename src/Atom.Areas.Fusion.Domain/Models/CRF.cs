using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class Crf : WorkItem
	{
		public virtual CrfStatus CrfStatus { get; set; }

		[Required(ErrorMessage = "Business Benefit is a required field")]
		public virtual string BusinessBenefit { get; set; }
		[Required(ErrorMessage = "Alternatives is a required field")]
		public virtual string Alternatives { get; set; }
		public virtual string OtherScope { get; set; }

		public override void AssignTo(User AssignTo, User AssignedBy)
		{
			CrfStatus = CrfStatus.InProgress;
			WorkStatus = WorkItemStatus.InProgress;
			base.AssignTo(AssignTo, AssignedBy);
		}

		public override void PutOnHold(User user)
		{
			if (!(CrfStatus > CrfStatus.Requested || CrfStatus < CrfStatus.InProgress)) return;
			CrfStatus = CrfStatus.OnHold;
			WorkStatus = WorkItemStatus.OnHold;
			base.PutOnHold(user);
		}

		public override void TakeOffHold(User user)
		{
			if (CrfStatus != CrfStatus.OnHold) return;
			CrfStatus = CrfStatus.InProgress;
			WorkStatus = WorkItemStatus.InProgress;
			base.TakeOffHold(user);
		}

		public virtual void UpdateSignOff(WorkItemSignOff signOff, User user)
		{
			var signOffToUpdate = SignOffs.FirstOrDefault(x => x.Id == signOff.Id);
			if (signOffToUpdate.Id <= 0 || signOffToUpdate.SignedOff != null)
				return;

			signOffToUpdate.SignedOffBy = user;
			signOffToUpdate.SignedOff = DateTime.Now;
			//Check all emegerency signoffs except one we are signing off now!
			var notAllEmergencySignedOff = SignOffs.Any(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance && !x.SignedOff.HasValue);

			if (WorkStatus == WorkItemStatus.AwaitingApproval && (signOff.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance || !notAllEmergencySignedOff))
			{
				WorkStatus = WorkItemStatus.InProgress;
				CrfStatus = CrfStatus.InProgress;
			}
			if (CanClose())
				CloseCrf(user);

		}



		private void CloseCrf(User user)
		{
			if (CrfStatus == CrfStatus.Completed) return;

			CrfStatus = CrfStatus.Completed;
			Close(user);
		}

		public virtual void CompleteCrf(User user, Crf crfToUpdateFrom)
		{
			if (CrfStatus == CrfStatus.Completed) return;

			EstimatedUnitsOfWork = crfToUpdateFrom.EstimatedUnitsOfWork ?? EstimatedUnitsOfWork;
			ActualUnitsOfWork = crfToUpdateFrom.ActualUnitsOfWork;
			EstimatedStartDate = crfToUpdateFrom.EstimatedStartDate ?? EstimatedStartDate;
			ImpactAnalysis = crfToUpdateFrom.ImpactAnalysis;
			CompletionComment = crfToUpdateFrom.CompletionComment ?? "";
			if (CanClose())
				CloseCrf(user);
		}

		public virtual void Reject(User user)
		{
			if (CrfStatus == CrfStatus.Rejected) return;

			CrfStatus = CrfStatus.Rejected;
			WorkStatus = WorkItemStatus.Closed;
			ClosedBy = user;
			Rejected = DateTime.Now;
			ClosedDate = DateTime.Now;
		}
	}
}
