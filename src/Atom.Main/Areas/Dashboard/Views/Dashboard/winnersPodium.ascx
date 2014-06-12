<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DashboardViewModel>" %>

<table cellpadding="0" cellspacing="0" border="0" width="100%" class="dashboard" align="center">
    <tr class="sideboardtitle">
        <td>
			<div class="textshadow">
				Completed
				<span id="Completed" class="stats"></span>
			</div>
		</td>
    </tr>
    <tr>
        <td class="SideboardItems" id="completed_crfs">
            <%foreach (var itemCompleted in Model.Completed)
            {%>
						<%=Html.Document(itemCompleted.Crf, itemCompleted.Title, itemCompleted.Severity, itemCompleted.Type, "")%>
						<%
            } %>
        </td>
    </tr>
</table>