<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DashboardViewModel>" %>
<%foreach (var item in Model.PMO_Team_Items)
{
%>
  <tr class="dashboarddata PmoTeam">
    <td class="<%=Model.Droppable %>AssignedTo sidebar textshadow" id="<%=item.LogonId %>" valign="middle" onclick="SwitchPmo()">
		<div style="width:100px;"></div>
		<%
			var user = Model.Session.Get<User>(Int32.Parse(item.LogonId)); %>
		<%=Html.Avatar(user, new { @class = "avatar" })%><br />
		<%=item.Nickname %>
		</td>

    <%foreach (var signoff in item.SignOffs)
    {%>
        <td width="12.5%" class="DashboardItems" id="<%=item.AssignedTo %>_<%=signoff.Status.Replace(" ","").Replace("&","") %>">
        <%foreach (var crf in signoff.Crfs)
        {
            if (crf.Number.Length > 0)
            {%>
						<%=Html.Document(crf.Number,crf.Title,crf.Severity,crf.Type,Model.Draggable)%>
						<%
            }
        }%>
        </td>    
    <%} %>

  </tr>
<%
} %>
