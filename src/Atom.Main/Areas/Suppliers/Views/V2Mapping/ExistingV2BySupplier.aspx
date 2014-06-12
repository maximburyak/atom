<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Suppliers.Models.ExistingV2MappingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers/Mappings --> ExistingBySupplier
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

			$('input[type="checkbox"][name="Disabled"]').click(function () {
				var msg = ($(this).attr('checked') ? 'disable' : 'enable');
				if (confirm('By confirming this mapping as being: ' + msg + 'd\nthis will ' + msg + ' this row in the system immediately\nContinue?')) {
					$(this).parent('form').submit();
				}
			});

			$('form[mapping="True"]').submit(function () {
				$.ajax({
					type: "POST",
					url: $(this).attr("action"),
					context: this,
					data: $(this).serialize(),
					success: function () {
						// TODO
					},
					error: function () {
						alert('Error has occurred:\nYour Mapping may not have been amended successfully!');
					}
				});
				return false;
			});
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Suppliers Classifications with Mappings for Validator2</h2>
	<fieldset style="width: 400px;">
		<legend><b>Search</b></legend>
		<%
			using (Html.BeginForm("ExistingV2BySupplier", "V2Mapping"))
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
					<label for="Disabled">
						Filter for Disabled?</label>
					<%=Html.DropDownList("Disabled",new SelectList(Model.DisabledValues,"value","value",Model.Disabled),"Choose") %>
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
				<th>
					V2 Category
				</th>
				<th>
					Disabled
				</th>
			</tr>
		</thead>
		<tbody>
			<%
			foreach (var i in Model.PagedList())
			{%>
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
					<%=string.IsNullOrEmpty(i.Category) ? "&nbsp;" : i.Category%>
				</td>
				<td valign="middle" align="center">
					<form mapping="True" method="post" action="AmendV2MappingDisabled">
					<%=Html.Hidden("SupplierID",Model.SupplierId) %>
					<%=Html.Hidden("Level1",i.Level1) %>
					<%=Html.Hidden("Category",i.Category) %>
					<%=Html.Hidden("Level2",i.Level2) %>
					<%=Html.Hidden("Level3",i.Level3) %>
					<input type="checkbox" name="Disabled" <%if (i.Disabled) {%>checked="checked" <%} %>
						id="Disabled" title="Disabled" value="true" />
					</form>
				</td>
			</tr>
			<%
				  
			}%>
		</tbody>
	</table>
	<div class="pager">
		<%=Html.Pager(Model.PagedList().PageSize, Model.PagedList().PageNumber, Model.PagedList().TotalItemCount, new { supplierid = Model.SupplierId, Model.Level1S,Model.Level2S,Model.Disabled })%>
	</div>
	<%
		}
		else
		{%>
	<p>
		No Items found!</p>
	<%} %>
</asp:Content>
