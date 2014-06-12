<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CaseDetailsBaseViewModel>" %>

<%if (RoleAuthorizationService.ShowClosureReason(Model.WorkItem.WorkItemType))
  { %>
<div id="case-reason">
<form id="frmclosurereason" method="post" action="/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/ClosureReason/<%=Model.WorkItem.Id%>">
<div>
	<h2>Reason for Support</h2>
	<span>
		<%=Html.DropDownList("closurereason", new SelectList(Model.ClosureReasons, "Id", "Description",Model.Incident.ClosureReason.Id), "Please Choose", new { style = "width:200px;margin-bottom:10px;" })%>
	</span>
	
	<div class="clear">
	</div>
</div>
</form>
    </div>
<% }
%>