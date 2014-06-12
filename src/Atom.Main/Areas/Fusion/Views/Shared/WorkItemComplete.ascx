<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>

<div id="workitem-complete">
	<%if (Request.IsAuthenticated)
   {
	   if (Model.WorkItem.WorkItemType != Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Incident)
	   {
		   if (RoleAuthorizationService.WorkItemCanSeeComplete(Model.WorkItem.WorkItemType))
		   {
			   if (Model.WorkItem.WorkStatus == Atom.Areas.Fusion.Domain.Models.WorkItemStatus.InProgress)
			   { %>
	<span>
		<button id="workitemcomplete" onclick="document.location.replace('/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/Complete/<%=Model.WorkItem.Id %>')">
			Complete
			<%=Model.WorkItem.WorkItemType.GetDescription().ToUpper()%></button>
	</span>
	<br />
	<%}
		}
   }
   }%>
</div>
