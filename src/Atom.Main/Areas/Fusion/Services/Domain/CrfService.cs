using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events;
using Atom.Areas.Fusion.Events.Crf;
using Atom.Areas.Fusion.Events.WorkItem;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class CrfService : WorkItemService
	{
		private readonly ISession _session;
		private readonly FusionCacheManager _cacheManager;
		private CommentService _commentService;

		public CrfService(ISession session)
			: base(session)
		{
			_session = session;
			_cacheManager = new FusionCacheManager(session);
			_commentService = new CommentService();
		}

		public AddCrfViewModel NewAddCrfViewModel(string name)
		{
			var model = new AddCrfViewModel(new Crf { CrfStatus = CrfStatus.Requested })
							{
                                Channels = Channels(),
                                InsuranceCompanies = InsuranceCompanies(),
								Suppliers = Suppliers(),
								ProductGroups = ProductGroups(),
								SignOffs = RequiredCrfSignOffs(),
								ChangeBoardMeetingDate = _changeBoardRepo.Get(1).NextMeeting.AddDays(7),
								User = GetUser(name)
							};
			return model;
		}

		public AddCrfViewModel NewAddCrfViewModel(Crf crf, string name)
		{
			var model = new AddCrfViewModel(crf)
			{
                Channels = Channels(),
                InsuranceCompanies = InsuranceCompanies(),
				Suppliers = Suppliers(),
				ProductGroups = ProductGroups(),
				SignOffs = RequiredCrfSignOffs(),
				ChangeBoardMeetingDate = _changeBoardRepo.Get(1).NextMeeting.AddDays(7),
				User = GetUser(name)
			};
			return model;
		}

		public AddCrfViewModel NewAddCrfViewModel(Crf crf, string name, IList<string> selectedchannelids, IList<string> selectedsupplierids, IList<string> selectedproductgroupids, IList<string> selectedinsurancecompanyids)
		{
			var model = new AddCrfViewModel(crf)
			{
                Channels = Channels(),
                InsuranceCompanies = InsuranceCompanies(),
				Suppliers = Suppliers(),
				ProductGroups = ProductGroups(),
				SignOffs = RequiredCrfSignOffs(),
				ChangeBoardMeetingDate = _changeBoardRepo.Get(1).NextMeeting.AddDays(7),
				User = GetUser(name)
			};

			model.SelectedChannels = Channels().Where(channel => selectedchannelids.Contains(channel.Id.ToString())).ToList();
			model.SelectedSuppliers = Suppliers().Where(supplier => selectedsupplierids.Contains(supplier.Id.ToString())).ToList();
			model.SelectedProductGroups = ProductGroups().Where(productgroup => selectedproductgroupids.Contains(productgroup.Id.ToString())).ToList();
			model.SelectedInsuranceCompanies = InsuranceCompanies().Where(insco => selectedinsurancecompanyids.Contains(insco.Id.ToString())).ToList();

			return model;
		}

		public void PutCrfOnHold(int id, string name)
		{
			var crf = GetCrf(id);
			var user = GetUser(name);
			crf.PutOnHold(user);
			UpdateWorkItem(crf, user);
		}

		public void TakeCrfOffHold(int id, string name)
		{
			var crf = GetCrf(id);
			var user = GetUser(name);
			crf.TakeOffHold(user);
			UpdateWorkItem(crf, user);
		}

		private IDictionary<int, string> RequiredCrfSignOffs()
		{
			return _cacheManager.RequiredCrfSignOffs();
		}


		public void AddCrf(Crf crf, IList<string> channels, IList<string> suppliers, IList<string> products, IList<string> insurancecompanies, string name, HttpFileCollectionBase files, bool emergencychange)
		{
			var user = GetUser(name);

			UpdateScopeDetails(crf, channels, suppliers, products, insurancecompanies, user);
			AddComments(crf, user);
			// Default to 6 weeks if no date provided.
			crf.RequestedCompletionDate = crf.RequestedCompletionDate ?? DateTime.Now.AddMonths(2);
			crf.WorkItemType = WorkItemTypeEnum.Crf;
			crf.WorkStatus = WorkItemStatus.AwaitingApproval;
			var handlingdepartment = GetHandlingDepartment(HandlingDepartmentTypeEnum.Pmo.GetDescription());
			crf.Department = handlingdepartment;
			//save list of required signoffs
			CreateListOfSignOffs(crf, user);
			Validator<Crf>.Validate(crf);

			_wiRepo.Save(crf);

			EmergencyChange(crf, user, emergencychange);

			if (files.Count > 0)
				AddDocument(crf.Id, user, files[0]);

			DomainEvents.Raise(new CrfAdded { Crf = crf, CreatedCrfUser = user });
		}

		public CrfDetailsViewModel CrfDetailsViewModel(int id, string name, bool added)
		{
			var crf = GetCrf(id);
			if (crf == null || crf.Id <= 0)
				throw new ArgumentOutOfRangeException("id", string.Format("No Crf exists with Id: {0}", id));

			var user = GetUser(name);

			if (user != null)
				base.AddHistory(id);

			var assignedToUser = "";
			if (crf.AssignedTo != null)
				assignedToUser = crf.AssignedTo.UserID;

			var model = new CrfDetailsViewModel(crf)
			{
				User = user,
				CommentAdded = added,
				ResourceUsers = ItUsers().Where(x => x.UserID != assignedToUser),
				LinkedWorkItems = LinkedWorkItems(id),
				SubscribeUsers = SubscribeUsers(crf),
                SubscribedUsers = SubscribedUsers(crf)
			};
			return model;
		}

		private Crf GetCrf(int id)
		{
			return new ListSingleCrf { id = id }
				.GetQuery(_session).List<Crf>().FirstOrDefault();
		}

		public CrfCompleteViewModel CrfCompleteViewModel(int id, string name, bool added)
		{
			var crf = GetCrf(id);

			if (crf == null)
				throw new ArgumentOutOfRangeException("id", string.Format("No Crf exists with Id: {0}", id));

			if (crf.Id <= 0)
				throw new ArgumentOutOfRangeException("id", string.Format("No Crf exists with Id: {0}", id));

			var model = new CrfCompleteViewModel(crf)
			{
				User = GetUser(name),
				CommentAdded = added
			};
			return model;
		}

		public int SubmitSignOff(int signOffId, string name, SeverityEnum? severity)
		{
			var signOff = _wiSignoffRepo.Get(signOffId);
			var crf = GetCrf(signOff.Workitem.Id);
			var user = GetUser(name);
			return SignOffItem(crf, signOff, severity, user);
		}

		private int SignOffItem(Crf crf, WorkItemSignOff signOff, SeverityEnum? severity, User user)
		{
			var signOffToUpdate = crf.SignOffs.FirstOrDefault(x => x.Id == signOff.Id);
			if (signOffToUpdate.Id <= 0 || signOffToUpdate.SignedOff.HasValue)
				return crf.Id;

			var severityChanged = false;
			var oldSeverity = crf.Severity;
			if (severity.HasValue)
			{
				severityChanged = severity.Value != crf.Severity;
			}

			crf.Severity = (severityChanged ? severity.Value : crf.Severity);

			//Add comment
			if (signOffToUpdate.SignOffType <= SignOffTypeEnum.ChangeBoardAcceptance)
			{
				AddSignOffComment(crf, user, oldSeverity, severityChanged, signOff.SignOffType);
			}

			crf.AlteredBy = user;
			crf.AlteredDate = DateTime.Now;
			crf.UpdateSignOff(signOff, user);
			DomainEvents.Raise(new WorkItemSignOffCompleted { WorkItem = crf });
			if (crf.CrfStatus == CrfStatus.Completed)
				DomainEvents.Raise(new CrfClosed() { Crf = crf });

			UpdateWorkItem(crf, user);
			return crf.Id;
		}

		private void AddSignOffComment(Crf crf, User user, SeverityEnum oldSeverity, bool severityChanged, SignOffTypeEnum signOffType)
		{
			var text = string.Format("This {0} has had a sign off of: {1} confirmed by {2} on behalf of the Change Board. {3}",
										 crf.WorkItemType.GetDescription().ToUpper(), signOffType.GetDescription(), user.Name,
										 (severityChanged ? "<br/>Severity changed from " + oldSeverity + " to " + crf.Severity : ""));
			var comment = new Comment
			{
				CommentText = text,
				Type = severityChanged ? CommentTypeEnum.SeverityChange : CommentTypeEnum.General,
				UnitsOfWork = 0
			};
			_commentRepo.Save(comment);
			_commentService.AddComment(crf, comment, user);
		}

		private void AddEmergencyCrfComment(Crf crf, User user)
		{
			var text = string.Format("This {0} is considered an emergency by: {1} which now requires 3 sign-offs.",
										 crf.WorkItemType.GetDescription().ToUpper(), user.Name);
			var comment = new Comment
			{
				CommentText = text,
				Type = CommentTypeEnum.General,
				UnitsOfWork = 0
			};
			_commentRepo.Save(comment);
			_commentService.AddComment(crf, comment, user);
		}

		public void CompleteCrf(int id, string name, Crf theCrf)
		{
			var user = GetUser(name);
			var crf = GetCrf(id);
			crf.CompleteCrf(user, theCrf);
			DomainEvents.Raise(new CrfCompletionDetailsUpdated { WorkItem = crf });
			UpdateWorkItem(crf, user);
		}

		public void RejectCrf(int id, WorkItemClosureReason ClosureReason, string name)
		{
			var crf = GetCrf(id);
			var user = GetUser(name);
			if (ClosureReason != WorkItemClosureReason.WorkCompleted)
			{
				var text = string.Format("{0} has been rejected by {1} for reason: {2}", crf.WorkItemType.GetDescription(), user.Name, ClosureReason.GetDescription());
				var comment = new Comment { CommentText = text, Type = CommentTypeEnum.SeverityChange, UnitsOfWork = 0 };
				_commentRepo.Save(comment);
				_commentService.AddComment(crf, comment, user);
			}

			crf.Reject(user);
			DomainEvents.Raise(new CrfClosed { Crf = crf });
		}



		public void EmergencyChange(int id, bool emergencychange, string userName)
		{
			var crf = GetCrf(id);
			var user = GetUser(userName);
			EmergencyChange(crf, user, emergencychange);
		}

		private void EmergencyChange(Crf crf, User user, bool emergencychange)
		{
			if (crf.CrfStatus == CrfStatus.Approved) return;

			var items = crf.SignOffs;
			var emergencySignOffsExist = (items.Count(x => x.SignOffType <= SignOffTypeEnum.EmergencyChange3) == 3);
			var normalSignOffExists = items.Any(x => x.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance);

			if (!emergencychange)
			{	//standard change via Change Board

				if (items.Any(x => x.SignOffType == SignOffTypeEnum.EmergencyChange1))
					_wiSignoffRepo.Delete(items.Where(x => x.SignOffType == SignOffTypeEnum.EmergencyChange1).First());

				if (items.Any(x => x.SignOffType == SignOffTypeEnum.EmergencyChange2))
					_wiSignoffRepo.Delete(items.Where(x => x.SignOffType == SignOffTypeEnum.EmergencyChange2).First());

				if (items.Any(x => x.SignOffType == SignOffTypeEnum.EmergencyChange3))
					_wiSignoffRepo.Delete(items.Where(x => x.SignOffType == SignOffTypeEnum.EmergencyChange3).First());

				if (!normalSignOffExists)
				{
					var approved = new WorkItemSignOff { SignOffType = SignOffTypeEnum.ChangeBoardAcceptance, Workitem = crf };
					_wiSignoffRepo.Save(approved);
				}
			}
			else
			{
				if (items.Any(x => x.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance))
					_wiSignoffRepo.Delete(items.Where(x => x.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance).First());

				// Only create Emergency sign offs if they do not already exist
				if (!emergencySignOffsExist)
				{
					_wiSignoffRepo.Save(new WorkItemSignOff { SignOffType = SignOffTypeEnum.EmergencyChange1, Workitem = crf });
					_wiSignoffRepo.Save(new WorkItemSignOff { SignOffType = SignOffTypeEnum.EmergencyChange2, Workitem = crf });
					_wiSignoffRepo.Save(new WorkItemSignOff { SignOffType = SignOffTypeEnum.EmergencyChange3, Workitem = crf });
				}
				AddEmergencyCrfComment(crf, user);
			}
			_wiRepo.Save(crf);
		}

		public int EmergencySignOff(int signOffId, string username)
		{
			var signOff = _wiSignoffRepo.Get(signOffId);
			var user = GetUser(username);
			var crf = GetCrf(signOff.Workitem.Id);
			var hasSignedOffEmergencyItem = crf.SignOffs.Any(x => x.SignOffType < SignOffTypeEnum.ChangeBoardAcceptance && x.SignedOffBy == user);

			// Prelim checks to see if the Crf has been approved, or its already been signed off, or they have already signed one of these off.
			if (crf.CrfStatus == CrfStatus.Approved || signOff.SignedOff.HasValue || hasSignedOffEmergencyItem)
				return crf.Id;

			return SignOffItem(crf, signOff, null, user);
		}
	}
}
