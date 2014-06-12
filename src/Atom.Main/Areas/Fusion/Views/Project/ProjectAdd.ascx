<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.AddProjectViewModel>" %>
<%@ Import Namespace="Atom.Main" %>

<%if (!RoleAuthorizationService.ProjectCreateView())
  {
	  Model.ErrorMessage = "Unfortunately you do not currently have access to \"Create Project\" function, please discuss any comments or concerns you may have with your line manager.";
%>
<%
	Html.RenderPartial("NotInRoleForFusion", Model); %>
<%}
  else
  { %>
<div class="box-error" id="messagebox" style="<%=Html.ErrorBoxDisplay(ViewData.ModelState.IsValid)%>">
	<h3>
		The following errors were encountered:</h3>
	<ul>
	</ul>
	<%=Html.ValidationSummary()%>
</div>
<br />
<% using (Html.BeginForm("Add", "Project", FormMethod.Post, new { enctype = "multipart/form-data", id = "addform" }))
   {%>
<%=Html.Hidden("Id","0") %>
<fieldset class="fs-project">
	<legend>Create Project Request</legend>
	<div>
		<label for="Summary">
			Project Title<span class="required-field"></span>
		</label>
		<%=Html.TextBox("Summary", "",new {maxLength=255}) %>
		<%= Html.ValidationMessage("Summary", "*")%>
	</div>
	<br />
	<div>
		<label for="RequestedCompletionDate">
			Completion Date
		</label>
		<%=Html.TextBox("RequestedCompletionDate", "")%>
		<%= Html.ValidationMessage("RequestedCompletionDate", "*")%>
	</div>
	<br />
	<div>
		<label for="ClientRequirement">
			Client Requirement?
		</label>
		<%=Html.CheckBox("ClientRequirement", false,new {style="width:30px;"}) %>
	</div>
	<br />
	<%if (RoleManager.IsUserInRole("Fusion.IT"))
   {%>
	<div>
		<label for="IsHouseKeeping">
			Is House Keeping?</label>
		<%= Html.CheckBox("IsHouseKeeping", false, new { style = "width:30px;" })%>
	</div>
	<br />
	<%}
   else
   {%>
	<%=Html.Hidden("IsHouseKeeping",false) %>
	<%} %>
	<div>
		<label for="CreateDate">
			Request Date
		</label>
		<%=Html.TextBox("CreateDate", DateTime.Now, new { @class = "input-readonly" })%>
		<%= Html.ValidationMessage("CreateDate", "*")%>
	</div>
	<br />
	<div>
		<label for="CreatedBy">
			Requested By
		</label>
		<%=Html.TextBox("CreatedBy",Model.User.Name, new {@class="input-readonly"})%>
		<%= Html.ValidationMessage("CreatedBy", "*")%>
	</div>
	<br />
	<div>
		<label>
			Description of Project<br />
			(min 50 characters)<span class="required-field"></span>
		</label>
		<div style="margin-left: 150px; width: 700px; background-color: White; padding: 5px;
			border: 1px solid #ccc;">
			<div style="">
				<%=Html.Hidden("Comments[0].UnitsOfWork","0") %>
				<div id="wmd-button-bar" class="wmd-panel">
				</div>
				<textarea style="position: relative; display: block;" id="wmd-input" class="wmd-panel"
					name="Comments[0].CommentText"><%if (Model.WorkItem.Comments != null)
									  { %><%=Model.WorkItem.Comments[0].CommentText %><%} %></textarea>
				<div id="wmd-preview" class="wmd-panel">
				</div>
				<br />
			</div>
		</div>
	</div>
	<br />
	<div>
		<label for="Severity">
			Severity Status <span class="required-field"></span>
		</label>
		<%= Html.DropDownList("Severity", new SelectList(Model.Severity(), "Key", "Value"), "Please choose Severity status")%>
		<%= Html.ValidationMessage("Severity", "*")%>
	</div>
	<br />
	<div>
		<label for="fileupload">
			Upload Attachment</label>
		<input type="file" class="case-document-upload" id="fileupload" name="fileupload"
			title="Browse for document" /></div>
	<button id="createproject" type="submit">
		Create</button>
</fieldset>
<% } %>
<%} %>
