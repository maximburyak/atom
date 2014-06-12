<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ERROR: Not In Role
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="fixed">
		<div id="pagetitle">
			<h1>
				Not In Role</h1>
		</div>
		<div class="box-error">
			<h3>
				Message: Insufficient Role Privileges
			</h3>
			<br />
			<p class="form-summary">
				<%=ViewData["Info"]%>
			</p>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
</asp:Content>
