using System;
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
	public class ProjectController : BaseController
	{
		private ProjectService _projectService;

		public ProjectController(ISession session)
			: base(session)
		{
			_projectService = new ProjectService(session);

		}

		// GET: /Crf/Details/5
		[HttpGet, CommentAdded, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.User, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Details(int id, string userName, bool commentAdded)
		{
			var model = _projectService.ProjectDetailsViewModel(id, userName, commentAdded);
			return View("Details", model);
		}

		// GET: /Crf/Details/5
		[HttpGet, CommentAdded, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.User, Fusion.CrfUser, Fusion.CrfAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Summary(int id, string userName, bool commentAdded)
		{
			var model = _projectService.ProjectDetailsViewModel(id, userName, commentAdded);
			return View("Summary", model);
		}

		// GET: /Crf/Complete/5
		[CommentAdded, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser")]
		public ActionResult Complete(int id, string userName, bool commentAdded)
		{
			var model = _projectService.ProjectCompleteViewModel(id, userName, commentAdded);
			return View("Complete", model);
		}

		// POST: /Crf/Complete
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser")]
		public ActionResult Complete(int id, string userName, Project project)
		{
			_projectService.CompleteProject(id, userName, project);
			return RedirectToAction("Complete", new { id, area = "Fusion" });
		}

		// GET: /Project/Add
		[HttpGet, ObtainUser]
		public ActionResult Add(string userName)
		{
			return View(_projectService.NewAddProjectViewModel(userName));
		}

		// GET: /Project/AddComment
		[HttpGet]
		public ActionResult AddComment(int id)
		{
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Project/AddComment/5
		[HttpPost, Transaction, ObtainUser, ValidateInput(false), AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult AddComment(int id, string CommentText, string userName, int UnitsOfWork)
		{
			try
			{
				_projectService.CreateComment(id, CommentText, userName, UnitsOfWork);
				TempData["CommentAdded"] = true;
				return RedirectToAction("Details", new { id, area = "Fusion" });
			}
			catch (Exception)
			{
				return View("Details");
			}
		}

		// POST: /Project/Add
		[HttpPost, Transaction, ObtainUser, ValidateInput(false), AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Add(Project project, string userName)
		{
			try
			{
				_projectService.AddProject(project, userName, Request.Files);
				return RedirectToAction("Details", new { id = project.Id, area = "Fusion" });
			}
			catch (RulesException ex)
			{
				ex.AddModelStateErrors(ModelState, null);
				return View(_projectService.NewAddProjectViewModel(project, userName));
			}
		}

		// POST: /Project/AddDocument
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult AddDocument(int id, string userName)
		{
			_projectService.AddDocument(id, userName, Request.Files[0]);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Project/AddDocument
		[HttpGet, Authorize(Order = 0), AuthorizeRole(Order = 2, Roles = "Fusion.CrfUser, Fusion.CrfAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult AddDocument(int id)
		{
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Project/Assign
		[ObtainUser, Transaction, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser,Fusion.ResourceUser")]
		public ActionResult Assign(int id, string userName, int? assignto)
		{
			_projectService.AssignWorkItem(id, userName, assignto, false);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET/POST: /Project/AssignToDept
		[Authorize(Order = 0), Transaction, AuthorizeRole(Roles = "Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.SuperUser,Fusion.ResourceUser", Order = 2), ObtainUser]
		public ActionResult AssignToDept(int id, string userName, int assigntodept)
		{
			_projectService.AssignWorkItemToDepartment(id, userName, assigntodept, false);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Project/Document/5
		[HttpGet, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Document(int id)
		{
			var document = _projectService.GetDocument(id);
			return File(document.Data, GetMimeType(document.FileName), document.FileName);
		}

		// GET: /Project/Subscribe/5
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Subscribe(int id, string userName)
		{
			_projectService.SubscribeUser(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// POST: /Project/SubscribeUser/Id
		[HttpPost, Transaction, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult SubscribeUser(int id, string subscribeuser)
		{
			_projectService.SubscribeUser(id, subscribeuser);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Project/Unsubscribe/5
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult Unsubscribe(int id, string userName, string fromController)
		{
			_projectService.UnsubscribeUser(id, userName);
			fromController = fromController ?? "Project";
			return RedirectToAction("Details", new { id, area = "Fusion", controller = fromController });
		}

		// GET: /Project/OnHold
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult OnHold(int id, string userName)
		{
			_projectService.PutWorkItemOnHold(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Project/OffHold
		[HttpGet, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.SMT, Fusion.COM, Fusion.SuperUser")]
		public ActionResult OffHold(int id, string userName)
		{
			_projectService.TakeWorkItemOffHold(id, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

		// GET: /Project/SignOff
		[HttpGet, Transaction, ObtainUser]
		public ActionResult SignOff(int id, string userName)
		{
			var crfId = _projectService.SubmitSignOff(id, userName, null);
			return RedirectToAction("Complete", new { id = crfId, area = "Fusion" });
		}

		//POST: /Project/SignOff
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.SMT")]
		public ActionResult SignOff(int id, SeverityEnum severity, string userName)
		{
			var crfId = _projectService.SubmitSignOff(id, userName, severity);
			return RedirectToAction("Details", new { id = crfId, area = "Fusion" });
		}

		//POST: /Project/Reject
		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Order = 2, Roles = "Fusion.SMT")]
		public ActionResult Reject(int id, WorkItemClosureReason CloseReason, string userName)
		{
			_projectService.RejectProject(id, CloseReason, userName);
			return RedirectToAction("Details", new { id, area = "Fusion" });
		}

	}


}
