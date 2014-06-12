<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProjectDetailsBaseViewModel>" %>

<%if (!RoleAuthorizationService.WorkItemCanSeeSignatures(Model.WorkItem.WorkItemType))
  {
	 %><p style="color:Red;">YOu do not have the required role to see signatures.</p><%
  }
  else
  {%>
<br />
<div class="project-complete">
	<h2>
		Signatures</h2>
	<p>
		Set signatures for the Project</p>
	<br />
	<div id="signaturelist">
		<%=Html.RequiredSignOffList(Model.User,Model.Project.SignOffs,Model.Project) %>
	</div>
</div>

<%} //End Role check for seeing signature details%>

