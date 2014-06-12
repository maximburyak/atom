using System.Web.Mvc;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using Atom.Main.Areas.Fusion.Services.Domain;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Controllers
{
	public class CABStatusController : BaseController
	{
		private readonly ProfileService _profileService;

		public CABStatusController(ISession session)
			: base(session)
		{
			_profileService = new ProfileService(session);
		}

		public PartialViewResult CABStatus()
		{
			var model = new CABStatusViewModel { ChangeBoardMeetingDate = _profileService.GetChangeBoardMeetingDate() };
			return PartialView("CABStatus", model);
		}
	}
}