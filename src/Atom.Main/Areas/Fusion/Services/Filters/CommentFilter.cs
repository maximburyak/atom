using System.Web.Mvc;

namespace Atom.Main.Areas.Fusion.Services.Filters
{
	public class CommentAddedAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			const string key = "commentAdded";
			filterContext.ActionParameters[key] = filterContext.Controller.TempData.ContainsKey(key);

			base.OnActionExecuting(filterContext);
		}
	}
}
