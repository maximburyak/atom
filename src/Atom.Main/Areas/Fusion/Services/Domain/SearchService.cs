using System;
using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Fusion.Data;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Data.Queries.Search.TokenManagers;
using Atom.Areas.Fusion.Data.Queries.Search.Tokens;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Models;
using BeValued.Data.NHibernate;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class SearchService
	{
		private readonly ISession _session;
		private readonly NHibernateRepository<SupportProfile> _profileRepo;
		private readonly NHibernateRepository<Filter> _filterRepo;
		private readonly List<TokenManager> _searchTokenManagers;
		private readonly ProfileService _profileService;
		public SearchService(ISession session)
		{
			_session = session;
			_profileRepo = new NHibernateRepository<SupportProfile>(_session);
			_filterRepo = new NHibernateRepository<Filter>(_session);
			_profileService = new ProfileService(_session);
			_searchTokenManagers = new List<TokenManager>
									{
									new PrefixTokenManager(),
									new DigitTokenManager(),
									new TextTokenManager()
									};
		}

		public WorkItemListModel Query(string search, int maxcase, string user)
		{

			if (String.IsNullOrEmpty(search))
				return AllWork(maxcase, user);

			var searchCriteria = new SearchQuery();
			var displayText = "";
			foreach (var tm in _searchTokenManagers)
			{
				tm.RunSearch(search);
				if (!tm.HasMatchedSearch()) continue;
				displayText = tm.DisplayText();
				searchCriteria = tm.CreateCriteria(maxcase);
				break;
			}

			return ListModel(searchCriteria.Criteria.GetExecutableCriteria(_session).List<WorkItem>(),
									 displayText, search);
		}

		private WorkItemListModel AllWork(int maxcase, string userName)
		{
			var user = string.IsNullOrEmpty(userName) ? null : GetUser(userName);
			HandlingDepartment dept = null;
			if (user != null && user.Department.Id >= 0)
				dept = user.Department;

			return ListModel(new ListOpenCases().GetQuery(_session, null, maxcase, dept), " With a status of Open ", "");
		}

		private WorkItemListModel AllWorkFiltered(int maxcase, string userName, WorkItemTypeEnum? type)
		{
			var user = string.IsNullOrEmpty(userName) ? null : GetUser(userName);
			HandlingDepartment dept = null;
			if (user != null && user.Department.Id >= 0)
				dept = user.Department;

			return ListModel(new ListOpenCases()
				.GetQuery(_session, type, maxcase, dept), string.Format(" With a status of {0} ", type.HasValue ? type.Value.GetDescription() : ""), "");
		}

		private WorkItemListModel Query(string userName, WorkItemTypeEnum? type, int maxcase, string displaytext, string searchMnemonic)
		{
			var items = new ListMyOpenCases().GetQuery(_session, type, maxcase, GetUser(userName));
			return ListModel(items, displaytext, searchMnemonic);
		}

		public WorkItemListModel MyWork(string name, int maxcase)
		{
			return Query(name, null, maxcase, " Open work, which belong to me ", "");
		}

		public WorkItemListModel MyIncidents(string name, int maxcase)
		{
			return Query(name, WorkItemTypeEnum.Incident, maxcase, " Open Incidents, which belong to me ", "");
		}

		public WorkItemListModel MyCrfs(string name, int maxcase)
		{
			return Query(name, WorkItemTypeEnum.Crf, maxcase, " Open Crfs, which belong to me ", "");
		}

		public WorkItemListModel MyProjects(string name, int maxcase)
		{
			return Query(name, WorkItemTypeEnum.Project, maxcase, " Open Projects, which belong to me ", "");
		}

		public WorkItemListModel LinkableWorkItems(string search, int maxcase, string name, int currentWorkItemId)
		{
			WorkItemListModel workitemlistmodel = Query(search, maxcase, name);
			workitemlistmodel.Items.Remove(workitemlistmodel.Items.Find(x => x.Id == currentWorkItemId));

			var workitemservice = new WorkItemService(_session);

			if (workitemservice.LinkedWorkItems(currentWorkItemId).Any())
				foreach (WorkItemLinkDto workitemlink in workitemservice.LinkedWorkItems(currentWorkItemId))
					workitemlistmodel.Items.Remove(workitemlistmodel.Items.Find(x => x.Id == workitemlink.RelatesToWorkItemId));

			return workitemlistmodel;
		}

		public WorkItemListModel OpenIncidents(string name, int maxcase)
		{
			return AllWorkFiltered(maxcase, name, WorkItemTypeEnum.Incident);
		}

		public WorkItemListModel OpenCrfs(string name, int maxcase)
		{
			return AllWorkFiltered(maxcase, name, WorkItemTypeEnum.Crf);
		}

		public WorkItemListModel OpenProjects(string name, int maxcase)
		{
			return AllWorkFiltered(maxcase, name, WorkItemTypeEnum.Project);
		}

		private static string GetWorkItemIndividualStatus(WorkItem c)
		{
			switch (c.WorkItemType)
			{
				case WorkItemTypeEnum.Crf:
					return ((Crf)c).CrfStatus.GetDescription();
				case WorkItemTypeEnum.Incident:
					return ((SupportIncident)c).IncidentStatus.GetDescription();
				case WorkItemTypeEnum.Project:
					return ((Project)c).Status.GetDescription();
				default:
					return "Unknown";
			}
		}

		private static WorkItemListModel ListModel(IEnumerable<WorkItem> items, string searchDisplayText, string searchFilter)
		{
			var model = new WorkItemListModel { SearchDisplayText = searchDisplayText, SearchFilter = searchFilter };
			var list = (from c in items
						select new WorkItemListItemModel
						{
							Id = c.Id,
							Summary = c.Summary.Left(140) + ((c.Summary.Length > 140) ? "..." : ""),
							Status = c.WorkStatus.GetDescription(),
							CreateDate = c.CreateDate.Value.ToString("dd MMMM yyyy HH:mm"),
							AssignAvatar = AssignAvatarForWorkItem(c),
							AvatarAltText = AssignAvatarAltTextForWorkItem(c),
							CreatedBy = c.CreatedBy.UserID,
							AlteredBy = c.AlteredBy != null ? c.AlteredBy.UserID : string.Empty,
							Area = (c.WorkItemType == WorkItemTypeEnum.Incident ? ((SupportIncident)c).System.Area.Description : ""),
							Controller = c.WorkItemType.GetDescription(),
							Priority = (int)c.Severity,
							LastUpdatedRelative = c.AlteredDate.HasValue ? c.AlteredDate.Value.ToString("dd MMMM yyyy HH:mm") : "",
							LastUpdatedFull = c.AlteredDate.HasValue ? c.AlteredDate.Value.FormatDateTimeFull() : "",
							WorkItemIndividualStatus = GetWorkItemIndividualStatus(c),
							WorkItemType = (int)c.WorkItemType,
							ClosedDate = (c.ClosedDate == null) ? "" : c.ClosedDate.Value.FormatDateTimeFull(),
							AssignedToName = (c.AssignedTo == null) ? "" : c.AssignedTo.Name,
							ClosedBy = (c.ClosedBy == null) ? "" : c.ClosedBy.Name,
							OnHoldImage = OnHoldImage(c),
							ProgressStatus = (c.WorkItemType != WorkItemTypeEnum.Incident ? SignoffStatus(c) : ""),
							DayOfWeekRaised = c.CreateDate.Value.ToString("dddd").Left(2),
							DayOfWeekRaisedFull = c.CreateDate.Value.ToString("dddd"),
							HouseKeeping = HouseKeepingImage(c),
							ClientRequirement = ClientRequirementImage(c),
                            InternalTesting = InternalTestingImage(c)
						})
					  .ToList();
			model.Items = list;
			return model;
		}

		private static string AssignAvatarAltTextForWorkItem(WorkItem c)
		{
			if (c.AssignedTo == null || c.AssignedTo.Profile == null || c.AssignedTo.Profile.CurrentAvatar == 0)
				return DeptAvatarAltTextForWorkItem(c);

			return c.AssignedTo.Name;
		}

		private static string DeptAvatarAltTextForWorkItem(WorkItem item)
		{
			var haveDept = item.Department.Id >= 0;
			var hasAssigned = item.AssignedTo != null;

			if (!hasAssigned && haveDept)
				return item.Department.Description;

			return "Missing Avatar{0}".With(hasAssigned ? " for " + item.AssignedTo.Name : "");
		}

		private static string AssignAvatarForWorkItem(WorkItem c)
		{
			if (c.AssignedTo == null || c.AssignedTo.Profile == null || c.AssignedTo.Profile.CurrentAvatar == 0)
				return DeptAvatarForWorkItem(c);

			return "{0}_{1}.{2}".With(c.AssignedTo.UserID, c.AssignedTo.Profile.CurrentAvatar,
										 c.AssignedTo.Profile.Avatars.Any(
											x =>
											!string.IsNullOrEmpty(x.FileExtension) && x.Id == c.AssignedTo.Profile.CurrentAvatar)
											? c.AssignedTo.Profile.Avatars.First(x => x.Id == c.AssignedTo.Profile.CurrentAvatar).
												FileExtension
											: "gif");
		}

		private static string DeptAvatarForWorkItem(WorkItem item)
		{
			var haveDept = item.Department.Id >= 0;
			var hasAssigned = item.AssignedTo != null;

			if (!hasAssigned && haveDept)
				return "dept-" + item.Department.Description + ".gif";

			return "missing.gif";
		}

		private static string SignoffStatus(WorkItem item)
		{
			if (!item.SignOffs.Any(x => !x.SignedOff.HasValue)) return "";

			var signOff = item.SignOffs.Where(x => !x.SignedOff.HasValue).Min(x => x.SignOffType);
			return signOff.GetDescription();
		}

		private static string OnHoldImage(WorkItem item)
		{
			var status = GetWorkItemIndividualStatus(item);
			// 1 = On Hold, 2 = Blank
			return status == "On Hold" ? "onhold.png" : "blank_28.png";
		}

		private static string HouseKeepingImage(WorkItem item)
		{
			return item.IsHouseKeeping ? "housekeeping.png" : "blank_1.png";
		}

		private static string ClientRequirementImage(WorkItem item)
		{
			return item.ClientRequirement ? "clientrequirements.png" : "blank_1.png";
		}

        private static string InternalTestingImage(WorkItem item)
        {
            return item.InternalTesting ? "internaltesting.png" : "blank_1.png";
        }

		public IList<Filter> Filters(string userid)
		{
			var filters = new ListUserFilters { UserID = userid }
				.GetQuery(_session).List<User>();
			return filters.Any() ? filters.First().Profile.Filters : new List<Filter>();
		}

		public Filter DefaultFilter(string userid)
		{
			var filters = Filters(userid);
			return filters.Any(x => x.IsDefault) ? filters.Where(x => x.IsDefault).FirstOrDefault() : new Filter() { FilterValue = "", Id = 0, Description = "" };
		}

		public User GetUser(string name)
		{
			var user = new ListSingleUser { name = name }
				.GetQuery(_session).List<User>().First();

			if (user.Profile == null)
			{
				_profileService.CreateProfile(user);
			}
			return user;
		}

		public bool FilterSave(string searchMnemonic, string searchFilter, string userid)
		{
			if (string.IsNullOrEmpty(searchMnemonic) || string.IsNullOrEmpty(searchFilter) || string.IsNullOrEmpty(userid))
				return false;

			var user = GetUser(userid);
			var filter = new Filter { Description = searchMnemonic, FilterValue = searchFilter };
			_filterRepo.Save(filter);

			if (user.Profile.Filters == null)
				user.Profile.Filters = new List<Filter>();

			user.Profile.Filters.Add(filter);
			_profileRepo.Save(user.Profile);
			return true;
		}
	}
}
