<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>
<% 
	if (RoleAuthorizationService.ProfileViewAutoAssignTo())
	{
%>
<hr style="color: #CCCCCC" />
<div class="autoassignto">
	<img alt="Auto assign to" id="autoassignto" src="/Areas/Fusion/Content/Images/icons/autoassign.png" />
	<h1>
		Auto assign Incidents to</h1>
	<div>
		<% using (Html.BeginForm("AutoAssignTo", "Profile", FormMethod.Post, new { id = "profileautoassignto" }))
	 {%>
		<p>
			<label class="formlabel" for="assignedDepartment">
				Departments:</label>
			<%=Html.DropDownList("assignedDepartment", new SelectList(Model.DropDownListDepartments, "Key", "Value", Model.AutoAssignedToDepartmentId), "Select Department..")%>
		</p>
		<p>
			<label class="formlabel" for="assignToDepartmentUser">
				Users:</label>
			<select id="assignToDepartmentUser" disabled="disabled">
			</select>
		</p>
		<p>
			<input id="saveAutoAssign" type="button" value="Assign" style="width: 75px; display: inline;" />
		</p>
		<%=Html.Hidden("assigneduser") %>
		<%} %>
	</div>
</div>
<%} %>
