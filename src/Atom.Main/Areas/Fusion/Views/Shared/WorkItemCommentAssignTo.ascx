<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (ViewData.Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
 {
 	if (Request.IsAuthenticated)
 	{
 		if (RoleAuthorizationService.WorkItemCanAssignToUser(Model.WorkItem.WorkItemType))
 		{%>
 		
 		<label  for="AssignTo" >Assign to yourself?</label>
 			<%=Html.CheckBox("AssignTo",false, new {style="width:30px;"}) %>
 			<br />
 		<%}//end role check
 	}//end auth check

 } //end closed check%>