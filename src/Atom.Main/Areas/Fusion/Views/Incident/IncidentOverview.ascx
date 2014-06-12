<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CaseDetailsBaseViewModel>" %>
<div id="case-overview">
	<h2>
		Overview</h2>
	<ul>
		<li>
			<h3>
				Support Dept.</h3>
			<%=Model.Incident.System.Area.Description%>
		</li>
		<li>
			<h3>
				Application</h3>
			<%if (Model.Incident.System.Category != null)
	 { %>
			<%=Model.Incident.System.Category.Description%>
			<%}
	 else
	 { %>
			N/A
			<%} %>
		</li>
		<li>
			<h3>
				Location</h3>
			<%=Model.Incident.Location.Name%>
		</li>
		
		<%if (Model.Incident.AdditionalInfo != null)
	{ %>
		<li>
			<h3>
				Additional Infomation</h3>
			<%if (!Model.Incident.AdditionalInfo.Any())
	 {%>
			No Additional Information
			<%} %>
			<%foreach (var ai in Model.Incident.AdditionalInfo)
	 {%>
			<label style="margin-right:5px;" for="<%="ai_"+ai.Id %>"><%=ai.InfoType.Description %>:</label><input name="<%="ai_"+ai.Id %>" id="<%="ai_"+ai.Id %>" type="text" readonly="readonly" style="width:100px;background-color:white;border:0;" value="<%=ai.Value %>" />
			<br />
			<%} %>
		</li>
		<%} %>
		<li>
			<h3>
				Client Requirement</h3>
			<%=Model.Incident.ClientRequirement%>
		</li>
		<li>
			<h3>
				House Keeping</h3>
			<%=Model.Incident.IsHouseKeeping%>
		</li>
        <li>
			<h3>
				Internal Testing</h3>
			<%=Model.Incident.InternalTesting%>
		</li>
	</ul>
</div>
