<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProjectDetailsBaseViewModel>" %>
<%if (Model.Project.Status == Atom.Areas.Fusion.Domain.Models.ProjectStatus.InProgress)
  {%>
<%Html.RenderPartial("ProjectCompletionDetails"); %>
<%}
  else if (Model.Project.Status == Atom.Areas.Fusion.Domain.Models.ProjectStatus.Completed)
  {%>
<%Html.RenderPartial("ProjectCompleted"); %>
<%}
  else
  {%>
<p style="margin-top: 10px;">
	This Project is not In Progress, and therefore can't be completed.</p>
<%}%>
<div class="clear">
</div>
