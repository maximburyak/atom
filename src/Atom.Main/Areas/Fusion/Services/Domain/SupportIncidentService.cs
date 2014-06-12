using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events;
using Atom.Areas.Fusion.Events.Incident;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using BeValued.Data.NHibernate;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class SupportIncidentService : WorkItemService
	{
		private readonly ISession _session;
		private readonly NHibernateRepository<SupportIncident> _caseRepo;
		private readonly NHibernateRepository<Details> _detailsRepo;
		private readonly NHibernateRepository<AdditionalInfo> _additionalInfoRepo;
		private readonly NHibernateRepository<AdditionalInfoType> _additionalInfoTypeRepo;
		private readonly NHibernateRepository<Category> _categoryRepo;
        private readonly NHibernateRepository<Area> _areaRepo;
        private readonly NHibernateRepository<Location> _locationRepo;
        private readonly NHibernateRepository<ClosureReason> _closureReasonRepo;
		private readonly FusionCacheManager _cacheManager;

		public SupportIncidentService(ISession session)
			: base(session)
		{
			_session = session;
			_wiRepo = new NHibernateRepository<WorkItem>(session);
			_wiRepo = new NHibernateRepository<WorkItem>(session);
			_caseRepo = new NHibernateRepository<SupportIncident>(session);
			_detailsRepo = new NHibernateRepository<Details>(session);
			_additionalInfoRepo = new NHibernateRepository<AdditionalInfo>(session);
			_additionalInfoTypeRepo = new NHibernateRepository<AdditionalInfoType>(session);
			_categoryRepo = new NHibernateRepository<Category>(session);
            _areaRepo = new NHibernateRepository<Area>(session);
            _locationRepo = new NHibernateRepository<Location>(session);
            _closureReasonRepo = new NHibernateRepository<ClosureReason>(session);
			_cacheManager = new FusionCacheManager(session);
		}

		public new User GetUser(string name)
		{
			return new ListSingleUser { name = name }
				.GetQuery(_session).List<User>().First();
		}

        public IEnumerable<ClosureReason> ClosureReasons(string handlingDepartmentId)
		{
            return _cacheManager.ClosureReasons(handlingDepartmentId);
		}

		public SupportIncident GetIncident(int id)
		{
			return _caseRepo.Get(id);
		}

        public Location GetLocationbyId(int id)
        {
            return _locationRepo.Get(id);
        }

		public SupportIncident GetIncidentByQuery(int id)
		{
			return new ListSingleIncident { id = id }.GetQuery(_session).List<SupportIncident>().FirstOrDefault();
		}

		public void CloseIncident(int id, string name)
		{
			var user = GetUser(name);
			var incident = GetIncident(id);

            var handlingDepartmentId = "";
            if (incident.Department != null)
                handlingDepartmentId = incident.Department.Id.ToString();

			incident.CloseCase(user);
			DomainEvents.Raise(new IncidentClosed { Incident = incident });
			UpdateWorkItem(incident);
		}

        public void AddClosureReason(int id, int reason)
        {
            var incident = GetIncident(id);
            var closurereason = _closureReasonRepo.Get(reason);
            incident.ClosureReason = closurereason;
            UpdateWorkItem(incident);
        }

		public void AddIncident(SupportIncident incident, string name, HttpFileCollectionBase files)
		{
			var currentuser = GetUser(name);
			Validator<SupportIncident>.Validate(incident);

			incident.System.Category = incident.System.Category.Id == 0 ? null : _categoryRepo.Get(incident.System.Category.Id);
			incident.System.Area = incident.System.Area.Id == 0 ? null : _areaRepo.Get(incident.System.Area.Id);
			if (incident.AdditionalInfo != null)
			{
				foreach (var ai in incident.AdditionalInfo)
				{
					_additionalInfoRepo.Save(ai);
				}
			}

			_detailsRepo.Save(incident.System);

			AddComments(incident, currentuser);
			if (incident.Comments != null)
			{
				foreach (var comment in incident.Comments)
				{
					//Remove script tags from numpty people
					comment.CommentText = Regex.Replace(comment.CommentText, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
					_commentRepo.Save(comment);
				}
			}

			incident.WorkItemType = WorkItemTypeEnum.Incident;
			incident.WorkStatus = WorkItemStatus.Open;

			_wiRepo.Save(incident);

			if (incident.System.Area != null)
			{
				// Get Dept for this Area, and Location
				var dept = new ListSingleIncidentDept { Area = incident.System.Area, Location = incident.Location }.GetQuery(_session).List<IncidentDepartment>().First();
				// Now go and get users for this Department
				var deptUser = new ListTeamUsers { isAutoAssignedTo = true, dept = incident.System.Area.HandlingDepartment.Description }.GetQuery(_session).List<User>().FirstOrDefault();

				// Assign to user, if found
				var autoUserFound = (deptUser != null && deptUser.Id > 0);

				if (autoUserFound)
				{
					AssignWorkItem(incident.Id, currentuser.UserID, deptUser.Id, true);
					incident.Department = dept.HandlingDepartment;
				}
				else
				{
					if (dept != null && dept.HandlingDepartment.Id > 0)
					{
						incident.Department = dept.HandlingDepartment;
						AssignWorkItemToDepartment(incident.Id, currentuser.UserID, dept.HandlingDepartment.Id, true);
					}
					else
					{
						AssignWorkItemToDepartment(incident.Id, currentuser.UserID, incident.System.Area.HandlingDepartment.Id, true);
					}
				}
			}

			if (files.Count > 0)
				AddDocument(incident.Id, currentuser, files[0]);

			DomainEvents.Raise(new IncidentAdded { Incident = incident });
		}

		public IEnumerable<Area> SupportArea()
		{
			return _cacheManager.SupportArea();
		}

		public AddCaseViewModel NewAddCaseViewModel(string name, string additionalinfo, string systemAreaId)
		{
			var model = new AddCaseViewModel(new SupportIncident { IncidentStatus = SupportIncidentStatus.Open })
			{
				SupportAreas = SupportArea().Where(x => x.Enabled).OrderBy(x => x.Description).ToList(),
				AreaCategories = SupportAreaCategories(systemAreaId),
				IncidentArea = IncidentArea(systemAreaId),
                Locations = Locations(),
				IncidentAdditionalInfo = IncidentAdditionalInfo(additionalinfo),
				User = GetUser(name)
			};
			return model;
		}

		/// <summary>
		/// Used to obtain additional information, passed in as a string.
		/// </summary>
		/// <param name="additionalinfo"></param>
		/// <returns></returns>
		private string IncidentAdditionalInfo(string additionalinfo)
		{
			if (string.IsNullOrEmpty(additionalinfo))
				return string.Empty;

			var info = additionalinfo.Split(',');

			var items = from i in info
						where !string.IsNullOrEmpty(i)
						select new { Id = i.Split(':')[0], Value = i.Split(':')[1] ?? "", Description = AdditionalInfoDescription(i.Split(':')[0]) };
			var oSerializer = new JavaScriptSerializer();
			var sJSON = oSerializer.Serialize(items.Where(x => x.Description != "" && x.Value != "").ToList());
			return sJSON;
		}

		public override void AssignWorkItem(int id, string name, int? assignto, bool autoassigned)
		{
			var workitem = GetIncident(id);
			workitem.IncidentStatus = SupportIncidentStatus.Open;
			_caseRepo.Update(workitem);
			base.AssignWorkItem(id, name, assignto, autoassigned);
		}

		public override void AssignWorkItemToDepartment(int id, string name, int assigntodept, bool autoassigned)
		{
			var workitem = GetIncident(id);
			workitem.IncidentStatus = SupportIncidentStatus.Open;
			_caseRepo.Update(workitem);
			base.AssignWorkItemToDepartment(id, name, assigntodept, autoassigned);
		}

		private string AdditionalInfoDescription(string additionalInfoTypeID)
		{
			var id = 0;
			if (!int.TryParse(additionalInfoTypeID, out id))
				return "";

			var item = _additionalInfoTypeRepo.Get(id);
			return item == null ? "" : item.Description ?? "";
		}

		private Area IncidentArea(string systemAreaId)
		{
			// default to empty list.
			if (string.IsNullOrEmpty(systemAreaId))
				return new Area();
			var s = 0;

			return int.TryParse(systemAreaId, out s) ? SupportArea().Where(x => x.Id == s).First() : new Area();
		}

		private IList<Category> SupportAreaCategories(string systemAreaId)
		{
			// default to empty list.
			if (string.IsNullOrEmpty(systemAreaId))
				return new List<Category>();
			var s = 0;
			return int.TryParse(systemAreaId, out s) ? SupportAreaCategories(s) : new List<Category>();
		}

		public AddCaseViewModel NewAddCaseViewModel(SupportIncident incident, string name)
		{
			var model = new AddCaseViewModel(incident)
			{
				SupportAreas = SupportArea().Where(x => x.Enabled).ToList(),
				User = GetUser(name)
			};
			return model;
		}

		private new IEnumerable<Channel> Channels()
		{
			return _cacheManager.Channels();
		}

        private new IEnumerable<Location> Locations()
        {
            return _cacheManager.SupportLocation();
        } 

		public void UpdateWorkItem(SupportIncident incident)
		{
			_caseRepo.Update(incident);
		}

		public void PutIncidentOnHold(int id, string name)
		{
			var incident = GetIncident(id);
			var user = GetUser(name);
			incident.PutOnHold(user);
			UpdateWorkItem(incident);
		}

		public void TakeIncidentOffHold(int id, string name)
		{
			var incident = GetIncident(id);
			var user = GetUser(name);
			incident.TakeOffHold(user);
			UpdateWorkItem(incident);
		}

		public IList<Category> SupportAreaCategories(int area)
		{
			return _cacheManager.SupportAreaCategories(area);
		}

		public IEnumerable<AdditionalInfoType> AdditionalInformation(int categoryId)
		{
			return _cacheManager.AdditionalInformation(categoryId);
		}

		public CaseDetailsViewModel CaseDetailsViewModel(int id, string name, bool commentAdded)
		{
			var incident = GetIncidentByQuery(id);
			if (incident == null)
				throw new ArgumentOutOfRangeException("id", string.Format("No Incident exists with Id: {0}", id));

			if (incident.Id <= 0)
				throw new ArgumentOutOfRangeException("id", string.Format("No Incident exists with Id: {0}", id));

            if(incident.ClosureReason==null)
                incident.ClosureReason=new ClosureReason();

			var user = GetUser(name);

            if (user != null)
				base.AddHistory(id);

			var assignedToUser = "";
			if (incident.AssignedTo != null)
				assignedToUser = incident.AssignedTo.UserID;

		    var handlingDepartmentId = "";
		    if (incident.Department != null)
		        handlingDepartmentId = incident.Department.Id.ToString();

			var model = new CaseDetailsViewModel(incident)
			{
				User = user,
				CommentAdded = commentAdded,
                ClosureReasons = ClosureReasons(handlingDepartmentId),
				LinkedWorkItems = LinkedWorkItems(id),
				ResourceUsers = ItUsers().Where(x => x.UserID != assignedToUser),
				SubscribeUsers = SubscribeUsers(incident),
                SubscribedUsers = SubscribedUsers(incident),
                SeverityList = new List<SeverityEnum>()
			};

			return model;
		}

		public void EditSummary(int id, string summary, string name)
		{
			var incident = GetIncidentByQuery(id);
			var user = GetUser(name);

			if (user != null && !string.IsNullOrEmpty(summary) && incident != null)
			{
				var comment = new Comment
								  {
									  CreatedBy = user,
									  CreateDate = DateTime.Now,
									  CommentText =
										  string.Format("Summary of Incident changed from:{0}\"{1}\"{0}to{0}\"{2}\"",
														"<p/>", incident.Summary, summary),
									  Type = CommentTypeEnum.General
								  };
				_commentRepo.Save(comment);
				incident.Comments.Add(comment);
				incident.Summary = summary;
				UpdateWorkItem(incident);
			}
		}
	}

}

