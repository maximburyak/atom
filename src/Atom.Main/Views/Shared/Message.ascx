<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% var msg = (Atom.Main.Models.ViewModels.MessageViewModel)TempData["message"];
   if (msg != null)
   {
	   var heading = (msg.IsError ? "error" : "info");
	   %>
<div id="message" class="<%=heading%>">
	<h2><%=heading.ToUpperInvariant() %></h2>
	<%=msg.Message %>
</div>
<%} %>