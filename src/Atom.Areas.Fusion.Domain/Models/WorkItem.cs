using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItem : AuditableFusionEntity
	{
		public WorkItem()
		{
			Init();
		}
		private void Init()
		{
			Subscriptions = new List<Subscription>();
		}
		public virtual int Id { get; set; }
		[Required(ErrorMessage = "Title is a required field"), StringLength(255, ErrorMessage = "Title must not exceed 255 characters")]
		public virtual string Summary { get; set; }
		public virtual WorkItemStatus WorkStatus { get; set; }
		public virtual WorkItemTypeEnum WorkItemType { get; set; }
		[Required(ErrorMessage = "Severity is a required field")]
		public virtual SeverityEnum Severity { get; set; }

		public virtual User AssignedTo { get; set; }
		public virtual DateTime? ClosedDate { get; set; }
		public virtual User ClosedBy { get; set; }
        public virtual ClosureReason ClosureReason { get; set; }
        public virtual bool IsHouseKeeping { get; set; }
        public virtual bool InternalTesting { get; set; }
		public virtual string CompletionComment { get; set; }
		public virtual string ImpactAnalysis { get; set; }
		public virtual DateTime? RequestedCompletionDate { get; set; }

		public virtual IList<WorkItemInsuranceCompany> InsuranceCompanies { get; set; }
		public virtual IList<WorkItemSupplier> Suppliers { get; set; }
		public virtual IList<WorkItemProductGroup> ProductGroups { get; set; }
		public virtual IList<Subscription> Subscriptions { get; set; }
		public virtual IList<Comment> Comments { get; set; }
		public virtual IList<Document> Documents { get; set; }
		public virtual IList<WorkItemSignOff> SignOffs { get; set; }
		public virtual HandlingDepartment Department { get; set; }
		public virtual DateTime? Rejected { get; set; }
		public virtual IList<WorkItemHistory> History { get; set; }

		public virtual int? EstimatedUnitsOfWork { get; set; }
		public virtual int? ActualUnitsOfWork { get; set; }

		public virtual DateTime? EstimatedStartDate { get; set; }
		public virtual bool ClientRequirement { get; set; }
		private string EmailToAddress { get; set; }


		public virtual void AddSubscription(Subscription subscription, User user)
		{
			Subscriptions.Add(subscription);
		}

		public virtual void AddHistory(WorkItemHistory itemHistory)
		{
			History.Add(itemHistory);
		}

		public virtual void AddDocument(Document document, User user)
		{
			Documents.Add(document);
		}

		public virtual void RemoveSubscription(Subscription subscription, User user)
		{
			Subscriptions.Remove(subscription);
		}

		public virtual void AddComment(int incidentId, Comment comment, User user)
		{
			Comments.Add(comment);
		}

		public virtual void AssignTo(User AssignTo, User AssignedBy)
		{
			AssignedTo = AssignTo;
			WorkStatus = WorkItemStatus.InProgress;
			Department = AssignTo.Department;
			//This must stay in, data used in raised event, prior to nhibernate flush.
			AlteredBy = AssignedBy;
			AlteredDate = DateTime.Now;
		}

		public virtual void AssignToDepartment(HandlingDepartment department, User assignedBy)
		{
			AssignedTo = null;
			WorkStatus = WorkItemStatus.InProgress;
			Department = department;
			//This must stay in, data used in raised event, prior to nhibernate flush.
			AlteredBy = assignedBy;
			AlteredDate = DateTime.Now;
		}

		public virtual void PutOnHold(User user)
		{
			WorkStatus = WorkItemStatus.OnHold;
		}

		public virtual void TakeOffHold(User user)
		{
			WorkStatus = WorkItemStatus.InProgress;
		}

		protected bool CanClose()
		{
			var allSignedOff = !SignOffs.Any(x => x.SignedOff == null);
			var allCompleted = EstimatedStartDate != null && EstimatedUnitsOfWork != null & ActualUnitsOfWork != null &&
							   !string.IsNullOrEmpty(ImpactAnalysis);
			return (allSignedOff && allCompleted);
		}

		protected void Close(User user)
		{
			WorkStatus = WorkItemStatus.Closed;
			ClosedBy = user;
			ClosedDate = DateTime.Now;
		}

		protected bool HasAssignedToUser()
		{
			return AssignedTo != null;
		}

		public virtual string EmailTo(User commentCreatedBy)
		{
			//send to Person who Raised WorkItem if they did not create comment
			EmailToAddress = CreatedBy == commentCreatedBy ? string.Empty : CreatedBy.EmailAddress;

			//send to Person assigned to WorkItem if already assigned and we dont already know who to send to
			if (HasAssignedToUser() && string.IsNullOrEmpty(EmailToAddress))
				EmailToAddress = (AssignedTo == commentCreatedBy) ? string.Empty : AssignedTo.EmailAddress;

			//comment must have been added by args.WorkItem.CreatedBy and WorkItem is not assigned yet
			//so send to First subcriber if there is one
			if (string.IsNullOrEmpty(EmailToAddress) && Subscriptions.Any())
				EmailToAddress = Subscriptions.First().User.EmailAddress;

			return EmailToAddress;
		}

		public virtual IList<string> EmailCc(User commentCreatedBy)
		{
			var emailAddresses = new List<string>();

			if (HasAssignedToUser() && EmailToAddress != AssignedTo.EmailAddress && commentCreatedBy.UserID != AssignedTo.UserID)
				emailAddresses.Add(AssignedTo.EmailAddress);

			emailAddresses.AddRange(Subscriptions.Where(subscription => subscription.User.EmailAddress != EmailToAddress).Select(subscription => subscription.User.EmailAddress));

			if (WorkItemType == WorkItemTypeEnum.Crf)
				emailAddresses.Add("changerequests@bevalued.co.uk");

			return emailAddresses;
		}
	}

}
