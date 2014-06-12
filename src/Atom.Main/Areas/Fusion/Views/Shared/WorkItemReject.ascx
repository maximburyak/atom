<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (RoleAuthorizationService.WorkItemCanReject(Model.WorkItem.WorkItemType))
  { %>
  
  <p>or</p>
  <br />
<form method="post" id="workitem-reject-form" action="/<%=Model.WorkItem.WorkItemType.GetDescription() %>/Reject/<%=Model.WorkItem.Id %>">
<span>
	<%=Html.DropDownList("CloseReason", new SelectList(Model.WorkItemClosureReasons(), "Key", "Value"), "Please Choose", new { style = "width:210px;margin-bottom:10px;" })%>
</span>
</form>
<span>
	<button id="rejectworkitem" disabled="disabled">
		Reject
	</button>
</span>
<div class="clear">
</div>
<% }%>
