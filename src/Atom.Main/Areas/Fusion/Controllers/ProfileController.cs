using System;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Fusion.Services.Filters;
using BeValued.Data.NHibernate.Mvc;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Controllers
{
	[Authorize(Order = 0)]
	public class ProfileController : BaseController
	{
		private readonly ProfileService _profileService;

		public ProfileController(ISession session)
			: base(session)
		{
			_profileService = new ProfileService(session);
		}

		[HttpGet, ObtainUser]
		public ActionResult Index()
		{
			ProfileViewModel model = _profileService.ProfileViewModel(User.Identity.Name);
			model.AutoAssignedToDepartmentId = _profileService.GetAutoAssignDepartmentId(User.Identity.Name);
			model.ResourceUsers = _profileService.GetUsersInMyDepartment(User.Identity.Name);
			model.DropDownListDepartments = _profileService.GetDropDownListDepartments();

			DateTime? date = _profileService.GetChangeBoardMeetingDate();
			model.ChangeBoardMeetingDate = date.HasValue ? date.Value : DateTime.Parse("31/12/9999");

			return View("Details", model);
		}

		[HttpGet]
		public ActionResult CreateSignature()
		{
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Details(int id)
		{
			return RedirectToAction("Index");
		}

		[HttpPost, Transaction, ActionName("Index"), ObtainUser]
		public ActionResult IndexPost(SupportProfile profile, string userName)
		{
			_profileService.AddAvatar(userName, profile, Request.Files);
			return RedirectToAction("Index");
		}

		[HttpPost, ObtainUser, Transaction,
		 AuthorizeRole(
			 Roles =
				 "Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 , Order = 2)]
		public ActionResult Signature(SupportProfile profile, string userName)
		{
			_profileService.AddSignature(userName, profile, Request.Files);
			return RedirectToAction("Index");
		}

		[HttpGet, Transaction, ObtainUser,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public ActionResult UpdateFilter(string userName)
		{
			_profileService.UpdateFilter(userName);
			return RedirectToAction("Index");
		}

		[HttpPost, Transaction, ObtainUser,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public ActionResult RemoveFilter(string userName, int filterid)
		{
			_profileService.RemoveFilter(userName, filterid, false);
			return RedirectToAction("Index");
		}

		[HttpPost, Transaction, ObtainUser,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public ActionResult FilterDefault(string userName, int filterdefault)
		{
			_profileService.FilterDefault(userName, filterdefault);
			return RedirectToAction("Index");
		}

		[HttpPost, Transaction, ObtainUser,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public ActionResult RemoveFilterDefault(string userName, int filterremove)
		{
			_profileService.RemoveFilter(userName, filterremove, true);
			return RedirectToAction("Index");
		}

		[HttpGet,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public ActionResult Unsubscribe(int id)
		{
			Subscription subscription = _profileService.GetSubscription(id);
			if (subscription != null)
			{
				string controller = subscription.WorkItem.WorkItemType.GetDescription();
				return RedirectToAction("Unsubscribe", controller,
										new
											{
												controller,
												subscription.WorkItem.Id,
												area = "Fusion",
												action = "Unsubscribe",
												fromController = "Profile"
											});
			}
			return RedirectToAction("Index", "Profile");
		}

		//POST: /Profile/AutoAssignIncidentTo
		[HttpGet, ObtainUser,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public JsonResult FindDepartmentUsersByDepartmentId(int id)
		{
			var data = _profileService.ListUsersByDepartment(id);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		//POST: /Profile/AutoAssignIncidentTo
		[HttpPost, Transaction, ObtainUser,
		 AuthorizeRole(
			 Roles =
				 "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser"
			 )]
		public ActionResult AutoAssignTo(int assignedDepartment, int assigneduser, string userName)
		{
			_profileService.AutoAssignToUser(userName, assignedDepartment, assigneduser);
			return RedirectToAction("Index", new { area = "Fusion" });
		}

		//POST: /Profile/UpdateChangeBoardMeetingDate
		[HttpPost, Transaction, AuthorizeRole(Roles = "Fusion.SMT, Fusion.SuperUser")]
		public ActionResult UpdateChangeBoardMeetingDate(DateTime? changeBoardMeetingDate)
		{
			if (changeBoardMeetingDate.HasValue)
				_profileService.UpdateChangeBoardMeeting(changeBoardMeetingDate.Value);

			return RedirectToAction("Index", new { area = "Fusion" });
		}

	}
}