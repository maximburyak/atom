<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
	if (Request.IsAuthenticated)
	{
%>
Welcome <b>
	<%= Html.Encode(Page.User.Identity.Name) %></b>! [
<%= Html.ActionLink("Log Out", "Logout", "Security", new {area = ""},null)%>
]
<%
	}
	else
	{
%>
[
<%= Html.ActionLink("Log In", "Login", "Security", new { area = "" }, null)%>
]
<%
	}
%>
