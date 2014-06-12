<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%@ Import Namespace="Atom.Main.Areas.Fusion.Services.Domain" %>
<%@ Import Namespace="BeValued.Utilities.Extensions" %>
<%if (Model.WorkItem.WorkItemType == Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Crf && !Model.WorkItemApprovalSignOffStarted() && Model.WorkItem.WorkStatus != Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
  { %>
<h3 style="padding-bottom: 5px;">
	Emergency Change?</h3>
<div id="emergencychangelist">
	<%if (RoleAuthorizationService.CrfCanSetEmergencyChange())
   {%>
	<form method="post" id="emergencyform" action="/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/EmergencyChange/<%=Model.WorkItem.Id %>">
	<label for="emergency">
		<span style="width: 50px;">Yes</span></label>
	<%=Html.RadioButton("emergencychange",true,Model.WorkItemIsEmergencyChange(),new {id="emergency",style="margin-left: 5px;width: 50px"}) %>
	<label for="noemergency">
		<span style="width: 50px;">No</span></label><%=Html.RadioButton("emergencychange", false, !Model.WorkItemIsEmergencyChange(), new { id = "noemergency", style = "margin-left: 5px;width: 50px" })%>
	<span>
		<button id="emergencybutton">
			Confirm
		</button>
	</span>
	</form>
	<%}
   else
   { %>
	<p style="color: Red;">
		Unfortunately you do not currently have access to Emergency Change details, please
		discuss any comments or concerns you may have with you line manager.</p>
	<%} %>
</div>
<%} %>
