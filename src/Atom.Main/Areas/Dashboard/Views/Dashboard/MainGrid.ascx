<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DashboardViewModel>" %>
<%@ Register Src="PmoGrid.ascx" TagName="PMO" TagPrefix="uc1" %>
<%@ Register Src="PmoTeamGrid.ascx" TagName="PMOTeam" TagPrefix="uc2" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
	<tr class="dashboardtitle">
		<td class="topbar-corner">&nbsp;</td>
		<%foreach (var item in Model.IT_Items[0].SignOffs)
	{
		%><td>
			<div class="textshadow"><%=item.Status %><span id="<%=item.Status.Replace(" ","").Replace("&","") %>" class="stats"></span></div>
		</td>
		<%
			} %>
	</tr>
	<uc1:PMO ID="Pmo1" runat="server" />
	<uc2:PMOTeam ID="Pmo2" runat="server" />
	<%foreach (var item in Model.IT_Items)
   {
	%>
	<tr class="dashboarddata">
		<td class="<%=Model.Droppable %>AssignedTo sidebar" id="<%=item.LogonId %>" valign="middle">
			<div style="width: 100px;">
			</div>
			<%
	var user = Model.Session.Get<User>(Int32.Parse(item.LogonId)); %>
			<div class="textshadow">
			<%=Html.Avatar(user, new { @class = "avatar" })%><br />
			<%=item.Nickname %>
			</div>
		</td>
		<%foreach (var signoff in item.SignOffs)
	{%>
		<td width="12.5%" class="DashboardItems" id="<%=item.AssignedTo %>_<%=signoff.Status.Replace(" ","").Replace("&","") %>">
			<%foreach (var crf in signoff.Crfs)
	 {
		 if (crf.Number.Length > 0)
		 {%><%=Html.Document(crf.Number,crf.Title,crf.Severity,crf.Type,Model.Draggable)%><%}
	 }%>
		</td>
		<%} %>
	</tr>
	<%
} %>
</table>
