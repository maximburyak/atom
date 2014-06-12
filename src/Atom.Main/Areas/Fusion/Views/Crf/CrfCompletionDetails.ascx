<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>
<%if (!RoleAuthorizationService.WorkItemCanComplete(Model.WorkItem.WorkItemType))
  {
%><p style="color: Red;">
	Unfortunately you do not currently have access to Complete CRF details, please discuss
	any comments or concerns you may have with your line manager.</p>
<%
	}
  else
  {%>
<div class="form">
	<div class="crf-complete">
		<%if (!Model.CanStartWorkOnCrf())
	{
		Html.RenderPartial("CrfCompletionDetails_Stage1");
	}
	else
	{
		Html.RenderPartial("CrfCompletionDetails_Stage2");
		%>
	</div>
</div>
<br />
<%Html.RenderPartial("CrfSignatures"); %>
<br />
<div id="progressnnote" class="crf-complete" s>
	<h2>
		Add Progress Note</h2>
	<%Html.RenderPartial("WorkItemComment"); %>
</div>
<%}%>
<%} //End Role check for editing completion details%>
