<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Atom.Main.Models.ViewModels.SecurityAdminViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom.Main -> Admin
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">

	<script type="text/javascript">
		$(function() {


			var msg = $('#message');
			if (msg.length > 0) {
				$(msg).fadeOut(2000);
			}

			$('#adduser').click(function() {
				$('#adduser-validation-summary').html('');
				if ($('#au_userID').val() != "") {
					$('#add_users').submit();
				}
				else {
					$('#adduser-validation-summary').html('Please fill in all fields').css("color", "red");
				}
			});

			$('#removeuser').click(function() {
				$('#removeuser-validation-summary').html('');
				if ($('#ru_userID').val() != "") {
					$('#remove_users').submit();
				}
				else {
					$('#removeuser-validation-summary').html('Please fill in all fields').css("color", "red");
				}
			});

			$('#addrole').click(function() {
				$('#addrole-validation-summary').html('');
				if ($('#ar_userID').val() != "" && $('#ar_roleName').val() != "") {
					$('#add_roles').submit();
				}
				else {
					$('#addrole-validation-summary').html('Please fill in all fields').css("color", "red");
				}
			});

			$('#removerole').click(function() {

				$('#removerole-validation-summary').html('');
				if ($('#rr_userID').val() != "" && $('#rr_roleName').val() != "") {
					$('#remove_roles').submit();
				}
				else {
					$('#removerole-validation-summary').html('Please fill in all fields').css("color", "red");
				}
			});

			$('#ar_userID').change(function() {
				var user = $('#ar_userID').val();
				$('#ar_roleName').attr("disabled", (user == ""));
				$.postJSON('/Security/RolesForUser', { userid: user }, function(data, textStatus) {
					$("#ar_roleName").fillSelect(data, 1);
				});
			});

			$('#rr_userID').change(function() {
				var user = $('#rr_userID').val();
				$('#rr_roleName').attr("disabled", (user == ""));
				$.postJSON('/Security/RolesUserHas', { userid: user }, function(data, textStatus) {
					$("#rr_roleName").fillSelect(data, 1);
				});
			});
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="form">
		<div id="pagetitle">
			<h1>
				Admin</h1>
		</div>
		<div class="box-content" style="width:50%">
			<h3 >
				System Access
			</h3>
			<%using (Html.BeginForm("AddUserToApplication", "Security", FormMethod.Post, new { id = "add_users" }))
	 {%>
			<div>
				<%=Html.ValidationSummary() %></div>
			<div class="form-value">
				<label for="au_userID">
					<span>Add User</span></label>
				<%=Html.DropDownList("au_userID", new SelectList(ViewData.Model.WildUsers, "userid", "FullName", ""), "Please Select")%>
				<input type="button" value="Add" name="adduser" id="adduser" />
				<span id="adduser-validation-summary">
					<%=Html.ValidationMessage("adduser")%></span>
			</div>
			<%} %>
			<%using (Html.BeginForm("RemoveUserFromApplication", "Security", FormMethod.Post, new { id = "remove_users" }))
	 {%>
			<div class="form-value">
				<label for="ru_userID">
					<span>Remove User</span></label>
				<%=Html.DropDownList("ru_userID", new SelectList(ViewData.Model.AtomUsers, "userid", "FullName", ""), "Please Select")%>
				<input type="button" value="Remove" name="removeuser" id="removeuser" />
				<span id="removeuser-validation-summary">
					<%=Html.ValidationMessage("removeuser")%></span>
			</div>
			<%
				}%>
		</div>
		<br />
		<!-- Role Form -->
		<div style="width:50%" class="box-content" >
			<h3>
				Role Access
			</h3>
			<%using (Html.BeginForm("AddUserToRole", "Security", FormMethod.Post, new { id = "add_roles" }))
	 {%>
			<div>
				<%=Html.ValidationSummary() %></div>
			<div class="form-value">
				<label for="ar_userID">
					<span>Add Role</span></label>
				<%=Html.DropDownList("ar_userID", new SelectList(ViewData.Model.AtomUsers, "userid", "FullName", ""), "Please Select")%>
				&nbsp;
				<%=Html.DropDownList("ar_roleName", new SelectList(ViewData.Model.HaveRoles, "value", "value", ""),
                                        "Please Select", new {disabled = true})%>
				<input type="button" value="Add" name="addrole" id="addrole" />
				<span id="addrole-validation-summary">
					<%=Html.ValidationMessage("addrole")%></span>
			</div>
			<%} %>
			<%using (Html.BeginForm("RemoveUserFromRole", "Security", FormMethod.Post, new { id = "remove_roles" }))
	 {%>
			<div>
				<%=Html.ValidationSummary() %></div>
			<div class="form-value">
				<label for="rr_userID">
					<span>Remove Role</span></label>
				<%=Html.DropDownList("rr_userID", new SelectList(ViewData.Model.AtomUsers, "userid", "FullName", ""), "Please Select")%>
				&nbsp;
				<%=Html.DropDownList("rr_roleName", new SelectList(ViewData.Model.HasRoles, "value", "value", ""),"Please Select", new {disabled = true})%>
				
				<input type="button" name="removerole" value="Remove" id="removerole" />
				<span id="removerole-validation-summary">
					<%=Html.ValidationMessage("removerole")%></span>
			</div>
			<%} %>
		</div>
		<!-- End form class -->
	</div>
	<%Html.RenderPartial("Message"); %>
</asp:Content>
