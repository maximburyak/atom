<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>

<%if (Model.Crf.CrfStatus == Atom.Areas.Fusion.Domain.Models.CrfStatus.InProgress)
  {%>
<%Html.RenderPartial("CrfCompletionDetails"); %>
<%}
  else if (Model.Crf.CrfStatus == Atom.Areas.Fusion.Domain.Models.CrfStatus.Completed)
  {%>
<%Html.RenderPartial("CrfCompleted"); %>
<%}
  else
  {%>
<p style="margin-top: 10px;">
	This Crf is not In Progress, and therefore can't be completed.</p>
<%}%>
<div class="clear">
</div>
