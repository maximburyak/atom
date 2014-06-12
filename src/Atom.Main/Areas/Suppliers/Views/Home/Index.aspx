<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Suppliers/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Suppliers --> Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Home</h2>
	<%if (ViewData["CacheMessage"] != null)
   {%>
	<p>
		<%=ViewData["CacheMessage"] %></p>
	<%} %>

	<p>Welcome to the Suppliers Administration Home. Use the links below to manage.</p>
</asp:Content>
