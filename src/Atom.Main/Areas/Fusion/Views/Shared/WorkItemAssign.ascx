<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<h2>
	Assigned To</h2>
<%
		
	Html.RenderPartial("WorkItemAssignTo");
	Html.RenderPartial("WorkItemAssigned");
%>
