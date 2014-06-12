<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.AddCaseViewModel>" %>
<%@ Import Namespace="Atom.Main" %>

<%if (!RoleAuthorizationService.IncidentCreateView())
  {
	  Model.ErrorMessage = "Unfortunately you do not currently have access to \"Create Incident\" function, please discuss any comments or concerns you may have with your line manager.";
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
<% using (Html.BeginForm("Add", "Incident", FormMethod.Post, new { enctype = "multipart/form-data", id = "addform" }))
   {%>
<%=Html.Hidden("Id","0") %>
<fieldset class="fs-incident">
	<legend style="margin-bottom: 20px; padding: 2px">Create Support Incident</legend>
	<p>
		<label for="Description">
			Description<span class="required-field"></span></label>
		<%= Html.TextBox("Summary") %>
		<%= Html.ValidationMessage("Summary","*")%>
	</p>
	<p>
		<label for="Location">
			Location<span class="required-field"></span></label>
		<%= Html.DropDownList("Location",new SelectList(Model.Locations,"Id","Name"),"Please choose Location") %>
		<%= Html.ValidationMessage("Location", "*") %>
	</p>
	<p>
		<label for="Severity">
			Severity<span class="required-field"></span></label>
		<%= Html.DropDownList("Severity",new SelectList(Model.Severity(),"Key","Value"),"Please choose Severity") %>
		<%= Html.ValidationMessage("Severity", "*")%>
	</p>
	<p>
		<label for="SupportArea">
			Support Department<span class="required-field"></span></label>
		<%= Html.DropDownList("System.Area.Id", new SelectList(Model.SupportAreas, "Id", "Description", Model.IncidentArea.Id), "Please choose Support Area")%>
		<%= Html.ValidationMessage("System.Area.Id", "*")%>
	</p>
	<p id="ApplicationRow">
		<label for="AreaCategory">
			Application</label>
		<%= Html.DropDownList("System.Category.Id", new SelectList(Model.AreaCategories, "Id", "Description"), "Please choose Application")%>
	</p>
	<p>
		<label for="ClientRequirement">
			Client Requirement?</label>
		<%=Html.CheckBox("ClientRequirement", false,new {style="width:30px;"}) %>
		
	</p>
    <%if (Model.User.Department.Id == (int)HandlingDepartmentTypeEnum.IT || Model.User.Department.Id == (int)HandlingDepartmentTypeEnum.Infrastructure || Model.User.Department.Id == (int)HandlingDepartmentTypeEnum.Guild || Model.User.UserID=="derekf")
   {%>
	<p>
		<label for="IsHouseKeeping">
			Is House Keeping?</label>
		<%= Html.CheckBox("IsHouseKeeping", false, new { style = "width:30px;" })%>
	</p>
	<%} else {%>
		<%=Html.Hidden("IsHouseKeeping",false) %>
	<%} %>
    <p>
		<label for="InternalTesting">
			Internal Testing?</label>
		<%= Html.CheckBox("InternalTesting", false, new { style = "width:30px;" })%>
	</p>
	<div id="additionalinformationcontainer">
		<h2>
			Additional Info:</h2>
		<br />
		<div id="predetermined-additionalinfo">
		</div>
		<div id="additionalinfocontainer">
			<div id="additionalinformation">
			</div>
		</div>
		<br />
	</div>
	<div>
		<h2>
			Summary <span class="required-field"></span>
		</h2>
	</div>
	<div class="workitem-comment">
		<%=Html.Hidden("Comments[0].UnitsOfWork","0") %>
		<div id="wmd-button-bar" class="wmd-panel">
		</div>
		<textarea id="wmd-input" class="wmd-panel" name="Comments[0].CommentText"><%if (Model.WorkItem.Comments != null)
																			  { %><%=Model.WorkItem.Comments[0].CommentText %><%} %></textarea>
		<div id="wmd-preview" class="wmd-panel">
		</div>
		<br />
	</div>
	<br />
	<p>
		<label for="fileupload">
			Upload Attachment</label>
		<input type="file" class="case-document-upload" id="fileupload" name="fileupload"
			title="Browse for document" /></p>
	<button id="createcase" type="button">
		Create</button>&nbsp;
	<button id="reset" type="reset">
		Reset</button>&nbsp;
		
</fieldset>
<% }

  }//End Role Check%>
