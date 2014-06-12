<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DashboardViewModel>" %>

<table class="stats">
<tr>
	<td>&nbsp;</td>
	<td class="qtytitle">CRF</td>
	<td class="qtytitle">PRJ</td>
</tr>
<%
	var rows = 0;
	const int maxrows = 5;
	foreach (var stat in Model.Stats1)
	{
		if(rows==maxrows)
		{
			%>
			</table>
			<table class="stats">
			<tr>
				<td>&nbsp;</td>
				<td class="qtytitle">CRF</td>
				<td class="qtytitle">PRJ</td>
			</tr>
			<%
			rows = 0;
		}
		%>
			<tr>
				<td class="title"><%=stat.Title %></td>
				<td class="qty" id="<%=stat.Title.Replace(" ","") %>_crf"><%=stat.QtyCrf %></td>
				<td class="qty" id="<%=stat.Title.Replace(" ","") %>_project"><%=stat.QtyProject %></td>
			</tr>
		<%
		rows++;

	}
	while (rows<maxrows)
	{
  %>
			<tr>
				<td class="title">&nbsp;</td>
				<td class="qty">&nbsp;</td>
				<td class="qty">&nbsp;</td>
			</tr>
		<%
		rows++;
	}
 %>
 </table>