<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DashboardViewModel>" %>

<table cellpadding="0" cellspacing="0" border="0" width="100%" class="dashboard" align="center">
    <tr class="sideboardtitle">
        <td>
			<div class="textshadow">
				UnAssigned
				<span id="UnAssigned" class="stats"></span>
			</div>
		</td>
    </tr>
    <tr>
        <td class="SideboardItems" id="unassigned_crfs">
            <%foreach (var UnAssigned in Model.Unassigned)
            {%>
						<%=Html.Document(UnAssigned.Crf, UnAssigned.Title, UnAssigned.Severity, UnAssigned.Type, Model.Draggable)%>
						<%
            }
						%>
        </td>
    </tr>
</table>