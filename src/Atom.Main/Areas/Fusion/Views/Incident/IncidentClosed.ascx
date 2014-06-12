<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CaseDetailsBaseViewModel>" %>
<div id="case-close">
	<h2>
		Closed By</h2>
	<%=Html.Avatar(Model.Incident.ClosedBy, new { @class = "avatar" })%>
	<span>
		<%=Model.Incident.ClosedBy.Name %></span>
	<ul>
		<li>
			<h3>
				Closure Date</h3>
			<%=Model.Incident.ClosedDate %>
		</li>
		<li>
			<h3>
				Reason</h3>
				<%=(Model.Incident.ClosureReason==null ? "Unknown" : Model.Incident.ClosureReason.Description) %>
		</li>
	</ul>
	<div class="clear">
	
	</div>
</div>
