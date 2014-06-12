using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Fusion.Data;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class BaseWorkItemViewModel
	{
		public WorkItem WorkItem { get; set; }
		public User User { get; set; }
		public IList<InsuranceCompany> InsuranceCompanies { get; set; }
		public IList<Supplier> Suppliers { get; set; }
		public IList<ProductGroup> ProductGroups { get; set; }
        public IList<Channel> Channels { get; set; }
        public IEnumerable<User> ResourceUsers { get; set; }
        public IList<SeverityEnum> SeverityList { get; set; }
		public IEnumerable<User> SubscribeUsers { get; set; }
        public IEnumerable<User> SubscribedUsers { get; set; }
		public IList<WorkItemLinkDto> LinkedWorkItems { get; set; }

		public string ErrorMessage { get; set; }

		public IDictionary<int, string> WorkItemClosureReasons()
		{
			var list = new WorkItemClosureReason().ToDictionary();
			list.Remove((int)WorkItemClosureReason.WorkCompleted);
			return list;
		}

		public IDictionary<int, string> Severity()
		{
			var list = new SeverityEnum().ToDictionary();
			return list;
		}

		public IDictionary<int, string> ResourceDepartments()
		{
			var list = new HandlingDepartmentTypeEnum().ToDictionary();
			return list;
		}

		public bool CanPerformTasks()
		{
			return WorkItem.WorkStatus > WorkItemStatus.AwaitingApproval && WorkItem.WorkStatus < WorkItemStatus.Closed;
		}

		public bool WorkItemHasBeenApproved()
		{
			var changeBoardAproved = WorkItem.SignOffs.Any(x => x.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance && x.SignedOff.HasValue);
			var emergencyApproved = WorkItem.SignOffs.All(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance && x.SignedOff.HasValue);
			// Item has been approved if the changeBoard has SignedOff Date
			// or All of the Emergency ones have.
			return changeBoardAproved || emergencyApproved;
		}

		public bool WorkItemChangeTypeIsKnown()
		{
			return WorkItem.SignOffs != null && WorkItem.SignOffs.Any(x => x.SignOffType <= SignOffTypeEnum.ChangeBoardAcceptance);
		}

		public bool WorkItemIsEmergencyChange()
		{
			return WorkItem.SignOffs != null && WorkItem.SignOffs.Any(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance);
		}

		public bool WorkItemApprovalSignOffStarted()
		{
			return WorkItem.SignOffs.Any(x => x.SignOffType <= SignOffTypeEnum.ChangeBoardAcceptance && x.SignedOff.HasValue);
		}
	}
}