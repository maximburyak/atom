<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CaseDetailsBaseViewModel>" %>
<%
	if (Model.Incident.IncidentStatus == Atom.Areas.Fusion.Domain.Models.SupportIncidentStatus.Closed)
	{
		Html.RenderPartial("IncidentClosed");
	}
%>