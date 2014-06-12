using System.Web.Mvc;
using Atom.Main.Filters;
using Atom.Main.Services.Filters;
using BeValued.Data.NHibernate.Mvc;

namespace Atom.Main.Areas.Suppliers.Controllers
{
	[RedirectToSsl(Order = -1), Authorize(Order = 0), HandleError, Transaction,
	AuthorizeRole(Order = 2, Roles = "Fusion.IT, Fusion.SupplierManager")]
	public class BaseController : Controller
	{
	}
}
