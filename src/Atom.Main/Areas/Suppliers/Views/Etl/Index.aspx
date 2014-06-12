<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<List<Atom.Areas.Suppliers.Domain.Models.EtlFilesProcessLogEntry>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers --> ETL v2
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="StyleContent" runat="server">
	<style type="text/css">
		table, .container
		{
			background: #FFFFFF;
			border: 1px solid;
			border-color: #404040 #ffffff #fafafa #303030;
			font-size: 10pt;
		}
		.container
		{
			padding: 6px;
		}
		thead
		{
			font-family: arial;
			font-weight: bold;
			font-size: 10pt;
			text-align: left;
		}
		
		th
		{
			border: 1px solid #000000;
			border-color: #ffffff #202020 #101010 #fafafa;
			background-color: #dcdcdc;
			padding-left: 4px;
			padding: 10px;
		}
		td
		{
			border-bottom: 1px solid #b0b0b0;
			border-right: 1px solid #b0b0b0;
			padding: 1px 2px 2px 4px;
			color: Black;
		}
	</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" ID="Content4" runat="server">

<script type="text/javascript">
	$(function () {
		$('button[name="etltoggle"]').click(function () {
			document.location.href = $(this).attr('action');
		});
	});
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		ETL v2</h2>
	<%if (Model.Any())
   {%>
	<table class="container1" cellpadding="0" cellspacing="0">
		<thead>
			<tr>
				<th align="left">
					SupplierId
				</th>
				<th align="left">
					File Type
				</th>
				<th align="left">
					Last File Processed
				</th>
				<th align="left">
					Last Processed
				</th>
				<th>
					Enabled
				</th>
				<th>
					&nbsp;
				</th>
			</tr>
		</thead>
		<%foreach (var i in Model)
{
		var action = i.Enabled ? "Disable" : "Enable";
		%>
		<tr>
			<td align="center">
				<%=i.SupplierId %>
			</td>
			<td>
				<%=i.FileType.GetDescription() %>
			</td>
			<td>
				<%=i.LastFileProcessed??string.Empty %>
			</td>
			<td style="width: 150px;">
				<%=i.LastProcessedRun.HasValue ? i.LastProcessedRun.Value.ToString() : string.Empty%>
			</td>
			<td align="center">
				<%=i.Enabled %>
			</td>
			<td align="center">
				<button action="Etl/<%=action %>/<%=i.Id %>/" name="etltoggle" >
					<%=action%>
				</button>
			</td>
		</tr>
		<%} %>
	</table>
	<%}
   else
   {%>
	No File record(s).
	<%} %>
</asp:Content>
