using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Fusion.Services.Filters;
using BeValued.Data.NHibernate.Mvc;
using NHibernate;
using xVal.ServerSide;

namespace Atom.Main.Areas.Fusion.Controllers
{
	[Authorize(Order = 0)]
	public class CrfController : BaseController
	{
		private CrfService _crfService;

		public CrfController(ISession session)
			: base(session)
		{
			_crfService = new CrfService(session);

		}

		// GET: /Crf/Add
		[HttpGet, ObtainUser]
		public ActionResult Add(string userName)
		{
			var model = _crfService.NewAddCrfViewModel(userName);
			return View(model);
		}

		// GET: /Crf/Details/5
		[HttpGet, CommentAdded, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.User, Fusion.CrfUser, Fusion.CrfAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Details(int id, string userName, bool commentAdded)
		{
			var model = _crfService.CrfDetailsViewModel(id, userName, commentAdded);
			return View("Details", model);
		}

		// GET: /Crf/Details/5
		[HttpGet, CommentAdded, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.User, Fusion.CrfUser, Fusion.CrfAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Summary(int id, string userName, bool commentAdded)
		{
			var model = _crfService.CrfDetailsViewModel(id, userName, commentAdded);
			return View("Summary", model);
		}

		// GET: /Crf/Complete/5
		[CommentAdded, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser")]
		public ActionResult Complete(int id, string userName, bool commentAdded)
		{
			var model = _crfService.CrfCompleteViewModel(id, userName, commentAdded);
			return View("Complete", model);
		}

		// POST: /Crf/Complete
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser")]
		public ActionResult Complete(int id, string userName, Crf crf)
		{
			_crfService.CompleteCrf(id, userName, crf);
			return RedirectToAction("Complete", new { id, area = "Fusion" });
		}

		// GET: /Crf/AddComment
		[HttpGet, Authorize(Order = 0)]
		public ActionResult AddComment(int id)
		{
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Crf/AddComment/5
		[HttpPost, Transaction, ObtainUser, ValidateInput(false), AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT,Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult AddComment(int id, string CommentText, string userName, int UnitsOfWork)
		{
			try
			{
				_crfService.CreateComment(id, CommentText, userName, UnitsOfWork);
				TempData["CommentAdded"] = true;
				return RedirectToAction("Details", new { id, area = "Fusion" });
			}
			catch (Exception)
			{
				return View("Details");
			}
		}

		// POST: /Crf/Add
		[HttpPost, Transaction, ObtainUser, ValidateInput(false), AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Add(Crf crf, string userName, IList<string> Channels, IList<string> Suppliers, IList<string> ProductGroups, IList<string> InsuranceCompanies, int emergencychange)
		{
			try
			{
				//Business rule: If the completion date requested is prior to the next change board meeting then a message appears stating that it should be logged as an emergency change
				_crfService.AddCrf(crf, Channels, Suppliers, ProductGroups, InsuranceCompanies, userName, Request.Files, emergencychange == 1);
				return RedirectToAction("Details", new { id = crf.Id, area = "Fusion" });
			}
			catch (RulesException ex)
			{
				ex.AddModelStateErrors(ModelState, null);
				return View(_crfService.NewAddCrfViewModel(crf, userName, Channels, Suppliers, ProductGroups, InsuranceCompanies));
			}
		}

		// POST: /Crf/AddDocument
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult AddDocument(int id, string userName)
		{
			_crfService.AddDocument(id, userName, Request.Files[0]);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Crf/AddDocument
		[HttpGet, Authorize(Order = 0), AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult AddDocument(int id)
		{
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Crf/Assign
		[ObtainUser, Transaction, AuthorizeRole(Order = 2, Roles = "Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser, Fusion.ResourceUser")]
		public ActionResult Assign(int id, string userName, int? assignto)
		{
			_crfService.AssignWorkItem(id, userName, assignto, false);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET/POST: /Crf/AssignToDept
		[Authorize(Order = 0), Transaction, AuthorizeRole(Roles = "Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser, Fusion.ResourceUser", Order = 2), ObtainUser]
		public ActionResult AssignToDept(int id, string userName, int assigntodept)
		{
			_crfService.AssignWorkItemToDepartment(id, userName, assigntodept, false);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Crf/Document/5
		[HttpGet, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Document(int id)
		{
			var document = _crfService.GetDocument(id);
			return File(document.Data, GetMimeType(document.FileName), document.FileName);
		}

		// GET: /Crf/Subscribe/5
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Subscribe(int id, string userName)
		{
			_crfService.SubscribeUser(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Crf/SubscribeUser/Id
		[HttpPost, Transaction, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult SubscribeUser(int id, string subscribeuser)
		{
			_crfService.SubscribeUser(id, subscribeuser);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Crf/Unsubscribe/5
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Unsubscribe(int id, string userName, string fromController)
		{
			_crfService.UnsubscribeUser(id, userName);
			fromController = fromController ?? "Crf";
			return RedirectToAction("Details", new { id, area = "Fusion", controller = fromController });
		}

		// GET: /Crf/OnHold
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult OnHold(int id, string userName)
		{
			_crfService.PutCrfOnHold(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Crf/OffHold
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult OffHold(int id, string userName)
		{
			_crfService.TakeCrfOffHold(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Crf/SignOff
		[HttpGet, Transaction, ObtainUser]
		public ActionResult SignOff(int id, string userName)
		{
			var crfId = _crfService.SubmitSignOff(id, userName, null);
			return RedirectToAction("Complete", new { id = crfId, area = "Fusion" });
		}

		//POST: /Crf/SignOff
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.SMT")]
		public ActionResult SignOff(int id, SeverityEnum severity, string userName)
		{
			var crfId = _crfService.SubmitSignOff(id, userName, severity);
			return RedirectToAction("Details", new { id = crfId, area = "Fusion" });
		}

		//POST: /Crf/Reject
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.SMT")]
		public ActionResult Reject(int id, WorkItemClosureReason CloseReason, string userName)
		{
			_crfService.RejectCrf(id, CloseReason, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		//POST: /Crf/EmergencyChange
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult EmergencyChange(int id, bool emergencychange, string userName)
		{
			_crfService.EmergencyChange(id, emergencychange, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		//POST: /Crf/EmergencySignOff
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.ChangeBoard")]
		public ActionResult EmergencySignOff(int id, string userName)
		{
			var crfId = _crfService.EmergencySignOff(id, userName);
			return RedirectToAction("Details", new { id = crfId, area = "Fusion" });
		}
	}
}
