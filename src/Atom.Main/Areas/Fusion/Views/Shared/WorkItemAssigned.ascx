<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>

<%if (Model.WorkItem.AssignedTo != null)
  {%>
<%=Html.AvatarForWorkItem(Model.WorkItem.AssignedTo, new { @class = "avatar" },Model.WorkItem)%>
<span>
	<%=Model.WorkItem.AssignedTo.Name%></span>
<%} %>
<%if (Request.IsAuthenticated)
  { %>
<%if (RoleAuthorizationService.WorkItemCanAssignToUser(Model.WorkItem.WorkItemType))
  {%>
<div style="margin-top: 10px;">
	<%if (Model.WorkItem.WorkStatus != Atom.Areas.Fusion.Domain.Models.WorkItemStatus.OnHold)
   { %>
	Assign to User:<br />
	<%using (Html.BeginForm("Assign", Model.WorkItem.WorkItemType.GetDescription(), new { id = Model.WorkItem.Id }, FormMethod.Post, new { @id = "assign-form" }))
   { %>
	<%=Html.DropDownList("assignto", new SelectList(Model.ResourceUsers, "Id", "Name"), "")%>
	or
	<br />
	<br />
	<%}%>
	Assign to Department:<br />
	<%using (Html.BeginForm("AssignToDept", Model.WorkItem.WorkItemType.GetDescription(), new { id = Model.WorkItem.Id }, FormMethod.Post, new { @id = "assigndept-form" }))
   { %>
	<%=Html.DropDownList("assigntodept", new SelectList(Model.ResourceDepartments().Where(x=>x.Value!="None"), "Key", "Value"), "")%>
	<%}
   }// end on hold check%>
</div>
<%} %>
<%}%>
