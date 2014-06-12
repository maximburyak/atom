using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Atom.Areas.Fusion.Data;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events;
using Atom.Areas.Fusion.Events.WorkItem;
using Atom.Main.Services;
using BeValued.Data.NHibernate;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class WorkItemService
	{
		private readonly ISession _session;
		protected NHibernateRepository<WorkItem> _wiRepo;
		protected readonly NHibernateRepository<WorkItemSignOff> _wiSignoffRepo;
		private readonly NHibernateRepository<Document> _documentRepo;
		protected readonly NHibernateRepository<Comment> _commentRepo;
		private readonly NHibernateRepository<WorkItemChannel> _wicRepo;
		private readonly NHibernateRepository<WorkItemSupplier> _wisRepo;
		private readonly NHibernateRepository<WorkItemProductGroup> _wipRepo;
		private readonly NHibernateRepository<WorkItemInsuranceCompany> _wiicRepo;
		private readonly NHibernateRepository<Subscription> _subscriptionRepo;
		private readonly NHibernateRepository<InsuranceCompany> _insuranceCompanyRepo;
		private readonly NHibernateRepository<Supplier> _supplierRepo;
		private readonly NHibernateRepository<ProductGroup> _productRepo;
		protected readonly NHibernateRepository<HandlingDepartment> _handlingdeptRepo;
		private readonly FusionCacheManager _cacheManager;
		private readonly CommentService _commentService;
		protected readonly NHibernateRepository<ChangeBoard> _changeBoardRepo;

		protected NHibernateRepository<WorkItemLink> _wilRepo;

		public WorkItemService(ISession session)
		{
			_session = session;
			_wiRepo = new NHibernateRepository<WorkItem>(_session);
			_documentRepo = new NHibernateRepository<Document>(_session);
			_commentRepo = new NHibernateRepository<Comment>(_session);
			_wicRepo = new NHibernateRepository<WorkItemChannel>(_session);
			_wisRepo = new NHibernateRepository<WorkItemSupplier>(_session);
			_wipRepo = new NHibernateRepository<WorkItemProductGroup>(_session);
			_wiicRepo = new NHibernateRepository<WorkItemInsuranceCompany>(_session);
			_subscriptionRepo = new NHibernateRepository<Subscription>(_session);
			_insuranceCompanyRepo = new NHibernateRepository<InsuranceCompany>(_session);
			_supplierRepo = new NHibernateRepository<Supplier>(_session);
			_productRepo = new NHibernateRepository<ProductGroup>(_session);
			_wiSignoffRepo = new NHibernateRepository<WorkItemSignOff>(_session);
			_handlingdeptRepo = new NHibernateRepository<HandlingDepartment>(_session);
			_cacheManager = new FusionCacheManager(session);
			_commentService = new CommentService();
			_changeBoardRepo = new NHibernateRepository<ChangeBoard>(_session);
			_wilRepo = new NHibernateRepository<WorkItemLink>(_session);
		}

		public virtual User GetUser(string name)
		{
			return new ListSingleUser { name = name }
				.GetQuery(_session).List<User>().First();
		}

		public virtual User GetUser(int id)
		{
			return new ListSingleUser { id = id }
				.GetQuery(_session).List<User>().First();
		}

		public virtual void AddDocument(int id, string name, HttpPostedFileBase file)
		{
			AddDocument(id, GetUser(name), file);
		}

		public virtual WorkItem GetWorkItem(int id)
		{
			return _wiRepo.Get(id);
		}

		public virtual void AddHistory(int id)
		{
			DomainEvents.Raise(new WorkItemHistoryEvent { WorkItem = GetWorkItem(id) });
		}

		public virtual void AddDocument(int id, User user, HttpPostedFileBase file)
		{
			var workitem = GetWorkItem(id);
			if (file == null) return;
			if (file.ContentLength <= 0) return;
			if (workitem.Documents == null) workitem.Documents = new List<Document>();

			var str = file.InputStream;
			var strLen = Convert.ToInt32(str.Length);
			var strArr = new byte[strLen];
			var filename = "";
			try
			{
				filename = Path.GetFileName(file.FileName);
			}
			catch (Exception)
			{
				filename = file.FileName;
			}

			var revision = RevisionNumber(filename, workitem);

			str.Read(strArr, 0, strLen);

			var document = new Document { Data = strArr, Revision = revision, FileName = filename };
			_documentRepo.Save(document);

			workitem.AddDocument(document, user);
			DomainEvents.Raise(new DocumentAdded { WorkItem = workitem, User = user });
			_wiRepo.Save(workitem);
		}

		public virtual void AddComments(WorkItem workitem, User user)
		{
			if (workitem.Comments == null) return;
			foreach (var comment in workitem.Comments)
			{
				//Remove script tags from numpty people
				comment.CommentText = Regex.Replace(comment.CommentText, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
				comment.Type = CommentTypeEnum.General;
				_commentRepo.Save(comment);
			}
		}
		public IList<ProductGroup> ProductGroups()
		{
			return _cacheManager.ProductGroups();
		}

		public IList<Supplier> Suppliers()
		{
			return _cacheManager.Suppliers();
		}

		public IList<InsuranceCompany> InsuranceCompanies()
		{
			return _cacheManager.InsuranceCompanies();
		}

		public IList<Channel> Channels()
		{
			return _cacheManager.Channels();
		}
		public void CreateComment(int id, string text, string name, int work)
		{
			var user = GetUser(name);
			var workitem = GetWorkItem(id);
			var comment = new Comment { CreatedBy = user, CommentText = text, UnitsOfWork = work };
			_commentRepo.Save(comment);
			_commentService.AddComment(workitem, comment, user);
			UpdateWorkItem(workitem, user);
		}

		protected void CreateListOfSignOffs(WorkItem workitem, User user)
		{
			if (workitem.WorkItemType != WorkItemTypeEnum.Crf)
			{
				var approved = new WorkItemSignOff { SignOffType = SignOffTypeEnum.ChangeBoardAcceptance, Workitem = workitem };
				_wiSignoffRepo.Save(approved);
			}
			var itAssign = new WorkItemSignOff { SignOffType = SignOffTypeEnum.Assigned, Workitem = workitem };
			var itPlan = new WorkItemSignOff { SignOffType = SignOffTypeEnum.PlanDesign, Workitem = workitem };
			var itDev = new WorkItemSignOff { SignOffType = SignOffTypeEnum.ITDevelopment, Workitem = workitem };
			var itpeer = new WorkItemSignOff { SignOffType = SignOffTypeEnum.ITPeerReview, Workitem = workitem };
			var itTest = new WorkItemSignOff { SignOffType = SignOffTypeEnum.ITUnitTesting, Workitem = workitem };
			var busTest = new WorkItemSignOff { SignOffType = SignOffTypeEnum.BusinessTesting, Workitem = workitem };
			var itRoll = new WorkItemSignOff { SignOffType = SignOffTypeEnum.ITRollout, Workitem = workitem };
			var busOwn = new WorkItemSignOff { SignOffType = SignOffTypeEnum.BusinessOwner, Workitem = workitem };

			_wiSignoffRepo.Save(itAssign);
			_wiSignoffRepo.Save(itPlan);
			_wiSignoffRepo.Save(itDev);
			_wiSignoffRepo.Save(itpeer);
			_wiSignoffRepo.Save(itTest);
			_wiSignoffRepo.Save(busTest);
			_wiSignoffRepo.Save(itRoll);
			_wiSignoffRepo.Save(busOwn);
			workitem.SignOffs = new List<WorkItemSignOff> { itAssign, itPlan, itTest, itpeer, busTest, itRoll, busOwn };
		}

		protected HandlingDepartment GetHandlingDepartment(string description)
		{
			return new ListSingleHandlingDept { description = description }.GetQuery(_session).List<HandlingDepartment>().First();
		}

		public virtual void UpdateWorkItem(WorkItem workItem, User user)
		{
			workItem.AlteredBy = user;
			workItem.AlteredDate = DateTime.Now;
			_wiRepo.Update(workItem);
		}

		public virtual void AssignWorkItem(int id, string name, int? assignto, bool autoassigned)
		{
			var workitem = GetWorkItem(id);
			var user = GetUser(name);
			workitem.AssignTo(assignto.HasValue ? GetUser((int)assignto) : user, user);
			if (!autoassigned)
				DomainEvents.Raise(new WorkItemAssigned { WorkItem = workitem });

			UpdateWorkItem(workitem, user);
		}

		public virtual void AssignWorkItemToDepartment(int id, string name, int assigntodept, bool autoassigned)
		{
			var workitem = GetWorkItem(id);
			var user = GetUser(name);
			var dept = _handlingdeptRepo.Get(assigntodept);
			if (dept == null) return;

			workitem.AssignToDepartment(dept, user);

			if (!autoassigned)
				DomainEvents.Raise(new WorkItemAssignedToDepartment { WorkItem = workitem }); ;

			UpdateWorkItem(workitem, user);
		}

		public Document GetDocument(int id)
		{
			return _documentRepo.Get(id);
		}

		public void SubscribeUser(int id, string name, bool autoassigned = false)
		{
			var workitem = GetWorkItem(id);
			var user = GetUser(name);
			// check to see if user is already subscribed
			if (workitem.Subscriptions.Any(x => x.User.Id == user.Id)) return;
			var subscription = new Subscription { User = user, WorkItem = workitem };
			_subscriptionRepo.Save(subscription);
			workitem.AddSubscription(subscription, user);
			if (!autoassigned)
				DomainEvents.Raise(new SubscriberAdded { WorkItem = workitem, NewSubscriber = user });

			UpdateWorkItem(workitem, user);
		}

		public void UnsubscribeUser(int id, string name)
		{
			var workitem = GetWorkItem(id);
			var user = GetUser(name);
			// check to see if user is already subscribed, if not continue
			if (!workitem.Subscriptions.Any(x => x.User.Id == user.Id)) return;
			var subscriptionId = workitem.Subscriptions.Where(x => x.User.Id == user.Id).FirstOrDefault().Id;
			var subscription = _subscriptionRepo.Get(subscriptionId);
			_subscriptionRepo.Delete(subscription);
			workitem.RemoveSubscription(subscription, user);
			DomainEvents.Raise(new SubscriberRemoved { WorkItem = workitem, Subscriber = user });
			UpdateWorkItem(workitem, user);
		}

		public void UpdateScopeDetails(WorkItem workitem, IEnumerable<string> channels, IEnumerable<string> suppliers, IEnumerable<string> products, IEnumerable<string> insurancecompanies, User user)
		{
			// 2. Suppliers
			if (workitem.Suppliers == null)
				workitem.Suppliers = new List<WorkItemSupplier>();

			foreach (var s in suppliers)
			{
				var supplier = _supplierRepo.Get(int.Parse(s));
				var wis = new WorkItemSupplier { Supplier = supplier };
				_wisRepo.Save(wis);
				workitem.Suppliers.Add(wis);
			}

			// 3. Products
			if (workitem.ProductGroups == null)
				workitem.ProductGroups = new List<WorkItemProductGroup>();

			foreach (var p in products)
			{
				var product = _productRepo.Get(int.Parse(p));
				var wip = new WorkItemProductGroup { ProductGroup = product };
				_wipRepo.Save(wip);
				workitem.ProductGroups.Add(wip);
			}

			// 4. Inscos
			if (workitem.InsuranceCompanies == null)
				workitem.InsuranceCompanies = new List<WorkItemInsuranceCompany>();

			foreach (var ic in insurancecompanies)
			{
				var iCompany = _insuranceCompanyRepo.Get(int.Parse(ic));
				var wiic = new WorkItemInsuranceCompany { InsuranceCompany = iCompany };
				_wiicRepo.Save(wiic);
				workitem.InsuranceCompanies.Add(wiic);
			}
		}

		private static decimal RevisionNumber(string name, WorkItem supportIncident)
		{
			return decimal.Round(supportIncident.Documents.Where(x => x.FileName == name).MaxOrDefault(x => x.Revision, 0.9m) + 0.1m, 1);
		}

		public void PutWorkItemOnHold(int id, string name)
		{
			var workitem = GetWorkItem(id);
			var user = GetUser(name);
			workitem.PutOnHold(user);
			UpdateWorkItem(workitem, user);
		}

		public void TakeWorkItemOffHold(int id, string name)
		{
			var workitem = GetWorkItem(id);
			var user = GetUser(name);
			workitem.TakeOffHold(user);
			UpdateWorkItem(workitem, user);
		}

		public IEnumerable<User> ItUsers()
		{
			return _cacheManager.ResourceUsers();
		}

		public IEnumerable<User> AllUsers()
		{
			return _cacheManager.AllUsers();
		}

		protected IEnumerable<User> SubscribeUsers(WorkItem item)
		{
			var allUsers = AllUsers();
			var subscriptions = (from s in item.Subscriptions
								 select s.User);
			var subscribeUsers = allUsers.Except(subscriptions, new SubscriptionUserEqualityComparer());
			return subscribeUsers;
		}
        
        protected IEnumerable<User> SubscribedUsers(WorkItem item)
        {
            var subscribedUsers = (from s in item.Subscriptions
                                 select s.User);
            return subscribedUsers;
        }

		public void LinkWorkItems(int id, int[] workItemsToLink)
		{
			var workitem = new WorkItem { Id = id };
			foreach (var wiid in workItemsToLink)
			{
				var workitemlink = new WorkItemLink { Item = workitem, RelatesTo = new WorkItem { Id = wiid } };
				_wilRepo.Save(workitemlink);
			}
		}

		public void UnLinkWorkItems(int id, int[] workItemsToUnLink)
		{
			foreach (var wiid in workItemsToUnLink)
			{
				var workitemlink = new WorkItemLink { Id = wiid };
				_wilRepo.Delete(workitemlink);
			}
		}

		public IList<WorkItemLinkDto> LinkedWorkItems(int id)
		{
			return new ListLinkedWorkItems { Id = id }.GetQuery(_session).List<WorkItemLinkDto>(); // (    .GetQuery(_session).List<WorkItemLink>().Select(x => x.RelatesTo).ToList()));
		}

	}
}
