using System;
using System.Linq;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Fusion.Services.Filters;
using BeValued.Data.NHibernate.Mvc;
using NHibernate;
using xVal.ServerSide;

namespace Atom.Main.Areas.Fusion.Controllers
{
	public class IncidentController : BaseController
	{
		private SupportIncidentService _incidentService;

		public IncidentController(ISession session)
			: base(session)
		{
			_incidentService = new SupportIncidentService(session);
		}

		// GET: /Incident/Details/5
		[HttpGet, CommentAdded, ObtainUser]
		public ActionResult Details(int id, string userName, bool commentAdded)
		{
			var model = _incidentService.CaseDetailsViewModel(id, userName, commentAdded);
			return View("Details", model);
		}

		// GET: /Incident/AddComment
		[HttpGet, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser, Fusion.ResourceUser", Order = 2)]
		public ActionResult AddComment(int id)
		{
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Incident/AddComment/5
		[HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser, Fusion.ResourceUser", Order = 2), ObtainUser, ValidateInput(false)]
		public ActionResult AddComment(int id, string CommentText, string userName, int UnitsOfWork, bool? AssignTo)
		{
			_incidentService.CreateComment(id, CommentText, userName, UnitsOfWork);
			if (AssignTo.HasValue && AssignTo.Value)
					_incidentService.AssignWorkItem(id, userName, null, false);
			
			TempData["CommentAdded"] = true;
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

        // POST: /Incident/AddComment/5
        [HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser, Fusion.ResourceUser", Order = 2), ObtainUser, ValidateInput(false)]
        public ActionResult AddCommentAndClose(int id, string CommentText, string userName, int UnitsOfWork, bool? AssignTo)
        {
            _incidentService.CreateComment(id, CommentText, userName, UnitsOfWork);
            _incidentService.CloseIncident(id, userName);
            
            if (AssignTo.HasValue && AssignTo.Value)
                 _incidentService.AssignWorkItem(id, userName, null, false);
            
            TempData["CommentAdded"] = true;
            return RedirectToAction("Details", new { id, area = "Fusion" });
        }

        // POST: /Incident/ClosureReason/5
        [HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser, Fusion.ResourceUser", Order = 2), ObtainUser, ValidateInput(false)]
        public ActionResult ClosureReason(int id, int closurereason)
        {
            _incidentService.AddClosureReason(id, closurereason);
            
            TempData["ClosureReasonAdded"] = true;
            return RedirectToAction("Details", new { id, area = "Fusion" });
        }

		// GET: /Incident/Add
		[HttpGet, Authorize(Order = 0)]
		public ActionResult Add(string additionalinfo, string systemid)
		{
			//Dont like this but it removes from QS.
			TempData["additionalinfo"] = additionalinfo;
			TempData["systemid"] = systemid;
			return RedirectToAction("Create", "Incident");
		}

		// GET: /Incident/Add
		[HttpGet, Authorize(Order = 0), ObtainUser]
		public ActionResult Create(string userName)
		{
			var additionalinfo = (string)TempData["additionalinfo"];
			var systemid = (string)TempData["systemid"];
			return View("Add", _incidentService.NewAddCaseViewModel(userName, additionalinfo, systemid));
		}

		// POST: /Incident/Add
		[HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2), ObtainUser, ValidateInput(false)]
		public ActionResult Add(SupportIncident supportIncident, string userName, int location)
		{
			try
			{
			    supportIncident.Location = _incidentService.GetLocationbyId(location);
                _incidentService.AddIncident(supportIncident, userName, Request.Files);
				return RedirectToAction("Details", new { id = supportIncident.Id, area = "Fusion" });
			}
			catch (RulesException ex)
			{
				ex.AddModelStateErrors(ModelState, null);
				return View(_incidentService.NewAddCaseViewModel(supportIncident, userName));
			}
		}

		// GET/POST: /Incident/Assign
		[Authorize(Order = 0), Transaction, AuthorizeRole(Roles = "Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.SuperUser, Fusion.ResourceUser", Order = 2), ObtainUser]
		public ActionResult Assign(int id, string userName, int? assignto)
		{
			_incidentService.AssignWorkItem(id, userName, assignto, false);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET/POST: /Incident/AssignToDept
		[Authorize(Order = 0), Transaction, AuthorizeRole(Roles = "Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.SuperUser, Fusion.ResourceUser", Order = 2), ObtainUser]
		public ActionResult AssignToDept(int id, string userName, int assigntodept)
		{
			_incidentService.AssignWorkItemToDepartment(id, userName, assigntodept, false);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Incident/OnHold
		[HttpGet, Authorize(Order = 0), Transaction, AuthorizeRole(Roles = "Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser", Order = 2), ObtainUser]
		public ActionResult OnHold(int id, string userName)
		{
			_incidentService.PutIncidentOnHold(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Incident/OffHold
		[HttpGet, Authorize(Order = 0), Transaction, AuthorizeRole(Roles = "Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser", Order = 2), ObtainUser]
		public ActionResult OffHold(int id, string userName)
		{
			_incidentService.TakeIncidentOffHold(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Incident/AreaCategories
		[HttpPost]
		public JsonResult AreaCategories(int supportArea)
		{
			var list = (from a in _incidentService.SupportAreaCategories(supportArea)
						select new { a.Id, a.Description }).ToList();
			return Json(list);
		}

		// POST: /Incident/CategoryAdditionalInfo
		[HttpPost]
		public JsonResult CategoryAdditionalInfo(int categoryId)
		{
			var list = (from a in _incidentService.AdditionalInformation(categoryId)
						select new { a.Id, a.Description }).ToList();
			return Json(list);
		}

		// GET: /Incident/AddDocument
		[HttpGet, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2)]
		public ActionResult AddDocument(int id)
		{
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Incident/AddDocument
		[HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2), ObtainUser]
		public ActionResult AddDocument(int id, string userName)
		{
			_incidentService.AddDocument(id, userName, Request.Files[0]);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Incident/EditSummary
		[HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.ResourceUser", Order = 2), ObtainUser]
		public ActionResult EditSummary(int id, string summary, string userName)
		{
			_incidentService.EditSummary(id, summary, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Incident/Document/5
		[HttpGet, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2)]
		public ActionResult Document(int id)
		{
			var document = _incidentService.GetDocument(id);
			return File(document.Data, GetMimeType(document.FileName), document.FileName);
		}

		// GET: /Incident/Subscribe/Id
		[HttpGet, Transaction, ObtainUser, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2)]
		public ActionResult Subscribe(int id, string userName)
		{
			_incidentService.SubscribeUser(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Incident/SubscribeUser/Id
		[HttpPost, Transaction, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2)]
		public ActionResult SubscribeUser(int id, string subscribeuser)
		{
			_incidentService.SubscribeUser(id, subscribeuser);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Incident/Unsubscribe/5
		[HttpGet, Transaction, ObtainUser, Authorize(Order = 0), AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CrfUser, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser", Order = 2)]
		public ActionResult Unsubscribe(int id, string userName, string fromController)
		{
			_incidentService.UnsubscribeUser(id, userName);
			fromController = fromController ?? "Incident";
			return RedirectToAction("Details", new { id, area = "Fusion", controller = fromController });
		}

		// GET: /Incident/Complete/5
		public ActionResult Complete(int id)
		{
			throw new NotImplementedException("Incidents are closed, not completed.");
		}
	}
}
