<%@ Control Language="C#" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<br />
<div id="workitem-hold">
	<%if (Request.IsAuthenticated)
   { %>
	<%
		if (RoleAuthorizationService.WorkItemChangeOnHoldStatus(Model.WorkItem.WorkItemType))
		{
			if (Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.OnHold)
			{ %>
	<span>
		<button id="caseonhold" onclick="document.location.replace('/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/OnHold/<%=Model.WorkItem.Id %>')">
			Place on Hold</button>
	</span>
	<%}
			else if (Model.WorkItem.WorkStatus == Atom.Areas.Fusion.Domain.Models.WorkItemStatus.OnHold)
			{ %>
	<span>
		<button id="caseoffhold" onclick="document.location.replace('/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/OffHold/<%=Model.WorkItem.Id %>')">
			Take off Hold</button>
	</span>
	<%}
		} //end role check%>
	<%}%>
</div>
<br />
