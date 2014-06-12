<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Stats.Models.ViewModels.ListWebLogViewModel>" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
if (Model.LogEntries.Count == 0)
{
%>There is no audit data that meets the above criteria.<%
}
else {
%>
<script type="text/javascript">
	$(document).ready(function () {
		attachColumnHiderHandler(".hideColumn", "#hiddenColumns");
	});
</script>

<table class="weblogResults" >
	<thead>
		<tr>
		<td><!-- Archive Button --></td> 
		<% // dynamically generate column names
			foreach (var property in Model.LogEntries.GetType().GetGenericArguments()[0].GetProperties())
			{
				string colName = property.Name;
				var attrs = (DisplayNameAttribute) Attribute.GetCustomAttribute(property, typeof (DisplayNameAttribute));
				if (attrs != null)
					colName = attrs.DisplayName;
				
				string sortClass = "";
				if (Model.SortColumn == property.Name)
					sortClass = (Model.IsSortDesc) ? "SortDesc" : "SortAsc";
				
				%>	
					<td 
						class="ClickSortable"
						columnname="<%=property.Name%>">
						<strong>
							<span 
								class="ClickSortableText <%=sortClass%>">
								<%=colName%>
							</span>
						</strong>
						<span>
						<%
							if (Model.SortColumn == property.Name)
								if (Model.IsSortDesc)
								{
									%><img class="UpArrow" src="../../Content/images/bullet_arrow_up.png" /><%
								}
								else
								{
									%><img class="DownArrow" src="../../Content/images/bullet_arrow_down.png" /><%
								}
						%>
						</span>
						<img src="../../Content/Images/cross.png" class="hideColumn" />
					</td>
				<%
			}
		%>
		</tr>
	</thead>
	<tbody>
		<%
			foreach (var logEntry in Model.LogEntries)
				Html.RenderPartial("WebLogEntry", logEntry);

				
		%>
	</tbody>
</table>
<button id="MoreResults">More</button>
<% 
} 
%>