<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (Request.IsAuthenticated)
  { %>
<%if (RoleAuthorizationService.WorkItemCanAssignToUser(Model.WorkItem.WorkItemType))
  {
	  if (Model.WorkItem.AssignedTo == null)
	  {
%>
<%=Html.AvatarForWorkItem(Model.WorkItem.AssignedTo, new { @class = "avatar", style="vertical-align:middle;" },Model.WorkItem)%>
<%
	  	if (Model.WorkItem.WorkStatus != Atom.Areas.Fusion.Domain.Models.WorkItemStatus.OnHold)
	  	{%>
<span style="margin-left: 4px; vertical-align: middle;">
	<button id="takecase" onclick="document.location.replace('/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription()%>/Assign/<%=Model.WorkItem.Id%>');">
		Take this Case</button>
</span>
<%
	  	}

	  } //end assigned to check%>
<%} %>
<%}%>
  