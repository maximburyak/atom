<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">

	<script type="text/javascript">
		$(function() {
			var msg = $('#message');
			if (msg.length > 0) {
				$(msg).fadeOut(4000);
			}
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div>
		<div class="form-login">
			<div id="pagetitle">
				<h1>
					Login</h1>
			</div>
			<%= Html.ValidationSummary() %>
			<% using (Html.BeginForm())
	  {%>
			<%=Html.Hidden("ReturnUrl",Request.QueryString["ReturnUrl"])%>
			<div class="box-content">
				<h3 style="width: 340px;">
					Log in to Atom
				</h3>
				<div class="form-value">
					<label for="username">
						<span>Username:</span></label>
					<%= Html.TextBox("username")%>
				</div>
				<div class="form-value">
					<label for="password">
						<span>Password:</span></label>
					<%= Html.Password("password")%>
				</div>
				<div class="form-value">
					<label for="rememberMe">
						<span>Remember me? </span></label>
					<%= Html.CheckBox("rememberMe",false)%>
				</div>
				<div>
					<button type="submit" name="submit" class="submit">
						Login</button>
				</div>
				
				<% } %>
			</div>
			<%Html.RenderPartial("Message"); %>
		</div>
	</div>
</asp:Content>
