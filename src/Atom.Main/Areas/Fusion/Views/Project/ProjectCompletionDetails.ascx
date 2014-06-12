<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProjectDetailsBaseViewModel>" %>

<%if (!RoleAuthorizationService.WorkItemCanComplete(Model.WorkItem.WorkItemType))
  {
%><p style="color: Red;">
	Unfortunately you do not currently have access to Complete project details, please discuss any comments or concerns you may have with your line manager.</p>
<%
	}
  else
  {%>
<div class="form">
	<div class="project-complete">
		<% using (Html.BeginForm("Complete", "Project", FormMethod.Post, new { enctype = "multipart/form-data", id = "addform" }))
	 {%>
		<div>
			<h2>
				Completion Details</h2>
		</div>
		<div>
			<label for="EstimatedUnitsOfWork">
				Estimated Time</label>
			<%=Html.TextBox("EstimatedUnitsOfWork", (Model.Project.EstimatedUnitsOfWork == null ? "" : Model.Project.EstimatedUnitsOfWork.ToString()), new { style = "width:100px" })%>
			<%=Html.ValidationMessage("EstimatedUnitsOfWork")%>
		</div>
		<div>
			<label for="ActualUnitsOfWork">
				Actual Time</label>
			<%=Html.TextBox("ActualUnitsOfWork", (Model.Project.ActualUnitsOfWork == null ? "" : Model.Project.ActualUnitsOfWork.ToString()), new { style = "width:100px" })%>
			<%=Html.ValidationMessage("ActualUnitsOfWork")%>
		</div>
		<div>
			<label for="EstimatedStartDate">
				Estimated Start Date</label>
			<%=Html.TextBox("EstimatedStartDate", Model.Project.EstimatedStartDate == null ? "" : ((DateTime)Model.Project.EstimatedStartDate).ToString("dd/MM/yyyy"), new { style = "width:100px" })%>
			<%=Html.ValidationMessage("EstimatedStartDate")%>
		</div>
		<div>
			<label for="CompletionComment">
				Completion Comments<br />
				Max 2000 chars</label>
			<textarea id="CompletionComment" name="CompletionComment" style="width: 350px;" rows="8"
				cols="5"><%=Model.Project.CompletionComment ?? ""%></textarea>
			<%=Html.ValidationMessage("CompletionComment")%>
		</div>
		<div>
			<button id="updatedetails" type="submit">
				Update Details</button>
		</div>
	</div>
</div>
<%} %>
<%} //End Role check for editing completion details%>
<br />
<%Html.RenderPartial("ProjectSignatures"); %>
<br />
<div id="progressnnote" class="project-complete" s>
	<h2>
		Add Progress Note</h2>
	<%Html.RenderPartial("WorkItemComment"); %>
</div>
