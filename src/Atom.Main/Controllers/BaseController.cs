using System;
using System.Web.Mvc;
using Atom.Main.Filters;
using Atom.Main.Models.ViewModels;

namespace Atom.Main.Controllers
{
	[RedirectToSsl(Order = -1), HandleError]
	public class BaseController : Controller
	{
		public const string TempDataMessageKey = "message";
		public void RedirectToActionMessage(bool isError, string message)
		{
			TempData[TempDataMessageKey] = new MessageViewModel { IsError = isError, Message = message };
		}

		public ActionResult NotInRole()
		{
			ViewData["Info"] = "Unfortunately you do not currently have access to this function, please discuss any comments or concerns you may have with your line manager.";
			return View();
		}

		public ActionResult ThrowError()
		{
			throw new NotImplementedException();
		}
	}

}
