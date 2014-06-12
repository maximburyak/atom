<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Suppliers.Models.MissingV2MappingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers/V2 Mappings --> Missing by Supplier
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
	<script type="text/javascript">

		$(function () {
			$('#submit').click(function () {
				if ($('#supplierid').val() == "") {
					alert('It helps if you choose a supplier..')
					return false;
				}
			});
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Supplier Feed Classifications without V2 Mappings (Non-Powerplay)</h2>
	<fieldset style="width: 400px;">
		<legend><b>Search</b></legend>
		
	<%
		using (Html.BeginForm("MissingV2BySupplier", "V2Mapping"))
		{%>
	<table class="form" cellpadding="0" cellspacing="0">
			<tr>
				<td valign="middle">
					<label for="supplierid">
						Select a Supplier</label>
					<%=Html.DropDownList("supplierid",new SelectList(Model.NonPpSuppliers,"Id", "Name",Model.SupplierId),"Please choose Supplier") %>
				</td>
				<td>
					&nbsp;
				</td>
				<td rowspan="3" valign="middle">
					<input id="submit" type="submit" value="Search" />
				</td>
			</tr>
			<tr>
				<td>
					<label for="Level1S">
						Filter for Level1:
					</label>
					<%=Html.TextBox("Level1S",Model.Level1S??string.Empty,new {maxLength=50}) %>
				</td>
			</tr>
			<tr>
				<td>
					<label for="Level2S">
						Filter for Level2:
					</label>
					<%=Html.TextBox("Level2S", Model.Level2S ?? string.Empty, new { maxLength = 50 })%>
				</td>
			</tr>
			<tr>
				<td>
					<label for="export">
						Export:
					</label>
					<input type="checkbox" value="true" id="export" name="export" />
				</td>
			</tr>
		</table>
	<%
		}%>
		</fieldset>
</asp:Content>
