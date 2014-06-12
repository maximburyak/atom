<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Suppliers.Models.MissingV2MappingPpViewModel>"
	MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master" %>

<%@ Import Namespace="Atom.Areas.Suppliers.Domain.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers/V2 Mappings --> Missing V2 Powerplay Mappings
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
	<script type="text/javascript">

		$(function () {
			$('#FormatS').change(function () {
				var $this = $(this);
				$('#FormatTypeS').val(null);
				$.ajax({
					type: "POST",
					url: '/Suppliers/Mapping/FormatTypesForFormat/',
					data: { format: $this.val() },
					success: function (data) {
						$('#FormatTypeS').html(data);
					},
					error: function () {
						alert('Error has occurred:\n!');
					}
				});

			});

		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		PP Suppliers Classifications without Mappings for Validator2</h2>
	<fieldset style="width: 500px;">
		<legend><b>Search</b></legend>
		<%
			using (Html.BeginForm("MissingV2PPBySupplier", "V2Mapping"))
			{%>
		<table class="form" cellpadding="0" cellspacing="0">
			
			<tr>
				<td>
					<label for="FormatS">
						Format:</label>
					<%=Html.DropDownList("FormatS", new SelectList(Model.Formats, "FormatCode", "FormatDescription"), "Choose Format")%>
				</td>
			</tr>
			<tr>
				<td>
					<label for="FormatTypeS">
						FormatType:</label>
					<%=Html.DropDownList("FormatTypeS", new SelectList(new List<FormatType>(), "FormatTypeCode", "Description"), "Choose FormatType")%>
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
			<tr>
				<td>
					<input id="submit" type="submit" value="Search" />
				</td>
			</tr>
		</table>
		<%
			}%>
	</fieldset>
</asp:Content>
