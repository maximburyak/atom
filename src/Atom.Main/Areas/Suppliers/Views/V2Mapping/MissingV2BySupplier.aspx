﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Suppliers.Models.MissingV2MappingViewModel>" %>
<%@ Import Namespace="Atom.Areas.Suppliers.Domain.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers/Mappings --> MissingV2 BySupplier
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

			$('select[mapping="True"]').change(function () {
				var formValid = $(this).parent('form').find('select[mandatory="True"] > option:selected[value=""]').length == 0;
				$(this).parent().children('button').attr("disabled", !formValid);
			});

			
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
					<%=Html.DropDownList("supplierid", new SelectList(Model.NonPpSuppliers, "Id", "Name", Model.SupplierId), "Please choose Supplier")%>
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
					<label for="Level1">
						Filter for Level1:
					</label>
					<%=Html.TextBox("Level1S",Model.Level1S??string.Empty,new {maxLength=50}) %>
				</td>
			</tr>
			<tr>
				<td>
					<label for="Level1">
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
					Level 1
				</th>
				<th align="left">
					Level 2
				</th>
				<th align="left">
					Level 3
				</th>
				<th align="left" style="width: 100px">
					Level 4
				</th>
				<th align="left">
					Classify
				</th>
			</tr>
		</thead>
		<tbody>
			<%
			var counter = 1;
			foreach (var i in Model.PagedList())
			{
			%>
			<tr>
				<td>
					<%=string.IsNullOrEmpty(i.Level1) ? "&nbsp;" : i.Level1%>
				</td>
				<td>
					<%=string.IsNullOrEmpty(i.Level2) ? "&nbsp;" : i.Level2%>
				</td>
				<td>
					<%=string.IsNullOrEmpty(i.Level3) ? "&nbsp;" : i.Level3%>
				</td>
				<td>
					<%=string.IsNullOrEmpty(i.Level4) ? "&nbsp;" : i.Level4%>
				</td>
				<td>
					<div style="position: relative;">
						<form mapping="True" method="post" action="SaveV2Mapping">
						<%=Html.Hidden("SupplierID",Model.SupplierId) %>
						<%=Html.Hidden("Level1",i.Level1) %>
						<%=Html.Hidden("Level2",i.Level2) %>
						<%=Html.Hidden("Level3",i.Level3) %>
						<%=Html.Hidden("Level4",i.Level4) %>
						
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
		<%=Html.Pager(Model.PagedList().PageSize, Model.PagedList().PageNumber, Model.PagedList().TotalItemCount, new { supplierid = Model.SupplierId, Model.Level1S, Model.Level2S })%>
	</div>
	<%
		}
		else
		{%>
	<p>
		No Items found!</p>
	<%} %>
</asp:Content>
