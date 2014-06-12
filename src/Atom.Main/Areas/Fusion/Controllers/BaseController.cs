using System.IO;
using System.Web.Mvc;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Fusion.Services.Filters;
using Atom.Main.Filters;
using BeValued.Data.NHibernate.Mvc;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Controllers
{
	[RedirectToSsl(Order = -1), AtomPrincipal(Order = 1), HandleError, AuthorizeRole(Roles = "Fusion.User", Order = 0)]
	public class BaseController : Controller
	{
		private readonly ISession _session;

		public BaseController(ISession session)
		{
			_session = session;
		}

		private WorkItemService _workItemService;

		public ActionResult NotInRole()
		{
			ViewData["Info"] = "Unfortunately you do not currently have access to this function, please discuss any comments or concerns you may have with your line manager.";
			return View("");
		}

		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser")]
		public ActionResult LinkWorkItems(int id, string userName, int[] selectedWorkItems)
		{

			object controller;
			RouteData.Values.TryGetValue("controller", out controller);

			if (selectedWorkItems != null)
				if (selectedWorkItems.Length > 0)
				{
					_workItemService = new WorkItemService(_session);
					_workItemService.LinkWorkItems(id, selectedWorkItems);
				}

			return RedirectToAction("Details", (string)controller, new { id, area = "Fusion" });
		}

		[HttpPost, Transaction, ObtainUser, AuthorizeRole(Roles = "Fusion.IncidentUser, Fusion.IncidentAdmin, Fusion.CRFUser, Fusion.CRFAdmin, Fusion.ProjectUser, Fusion.ProjectAdmin, Fusion.IT, Fusion.COM, Fusion.SMT, Fusion.SuperUser")]
		public ActionResult UnLinkWorkItems(int id, string userName, int[] selectedLinkedWorkItems)
		{
			object controller;
			this.RouteData.Values.TryGetValue("controller", out controller);

			if (selectedLinkedWorkItems != null)
				if (selectedLinkedWorkItems.Length > 0)
				{
					_workItemService = new WorkItemService(_session);
					_workItemService.UnLinkWorkItems(id, selectedLinkedWorkItems);
				}

			return RedirectToAction("Details", (string)controller, new { id, area = "Fusion" });
		}

		public string GetMimeType(string fileName)
		{
			var mimeType = "application/unknown";
			string ext = Path.GetExtension(fileName).ToLower();
			Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
			if (regKey != null && regKey.GetValue("Content Type") != null)
			{
				mimeType = regKey.GetValue("Content Type").ToString();
			}
			else if (ext == ".png") // a couple of extra info, due to missing information on the server
			{
				mimeType = "image/png";
			}
			else if (ext == ".flv")
			{
				mimeType = "video/x-flv";
			}
			return mimeType;
		}

	}
}
