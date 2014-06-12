<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Suppliers.Models.MissingV2MappingPpViewModel>" %>
<%@ Import Namespace="Atom.Areas.Suppliers.Domain.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers/Mappings --> MissingV2 BySupplier
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
	<script type="text/javascript">

		$(function () {

			$('select[mapping="True"]').change(function () {
				var formValid = $(this).parent('form').find('select[mandatory="True"] > option:selected[value=""]').length == 0;
				$(this).parent().children('button').attr("disabled", !formValid);
			});


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

			$('#FormatS').change();

			$('form[mapping="True"]').submit(function () {
				$.ajax({
					type: "POST",
					url: $(this).attr("action"),
					context: this,
					data: $(this).serialize(),
					success: function () {
						$(this).find('button,select,input').attr("disabled", true);

					},
					error: function () {
						alert('Error has occurred:\nYour Mapping may not have saved successfully!');
					}
				});
				return false;
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
	<%
		if (Model.PagedList().Count > 0)
		{%>
	<p>
		Found
		<%=Model.PagedList().TotalItemCount%>
		items.
	</p>
	<table class="grid" style="width: 100%">
		<thead>
			<tr>
				<th align="left">
					Format
				</th>
				<th align="left">
					FormatType
				</th>
				<th align="left">
					Class
				</th>
				<th align="left">
					Classify
				</th>
			</tr>
		</thead>
		<tbody>
			<%
			var counter = 1;
			foreach (var i in Model.PagedList().OrderBy(x=>x.Format))
			{
			%>
			<tr>
				<td>
					<%=string.IsNullOrEmpty(i.FormatDescription) ? "&nbsp;" : i.FormatDescription%>
				</td>
				<td>
					<%=string.IsNullOrEmpty(i.FormatTypeDescription) ? "&nbsp;" : i.FormatTypeDescription%>
				</td>
				<td>
					<%=string.IsNullOrEmpty(i.ClassDescription) ? "&nbsp;" : i.ClassDescription%>
				</td>
				
				<td>
					<div style="position: relative;">
						<form mapping="True" method="post" action="SaveV2PPMapping">
						<%=Html.Hidden("Format", i.Format)%>
						<%=Html.Hidden("FormatType", i.FormatType)%>
						<%=Html.Hidden("Class", i.Class)%>
						
						<%=Html.DropDownList("Category", new SelectList(Model.Categories, "Category", "Category"), "Choose Category", new { mapping = true, mandatory = true, id = "Category_" + counter })%>
						
						<label for="Disabled">
							Disabled?</label>
						<input type="checkbox" id="Disabled" name="Disabled" value="True" />
						<button style="float: left; position: absolute; top: 0; right: 0;" type="submit"
							disabled="disabled">
							Save
						</button>
						</form>
					</div>
				</td>
			</tr>
			<%
				counter++;
			}%>
		</tbody>
	</table>
	<div class="pager">
		<%=Html.Pager(Model.PagedList().PageSize, Model.PagedList().PageNumber, Model.PagedList().TotalItemCount, new { Model.FormatS, Model.FormatTypeS })%>
	</div>
	<%
		}
		else
		{%>
	<p>
		No Items found!</p>
	<%} %>
</asp:Content>
