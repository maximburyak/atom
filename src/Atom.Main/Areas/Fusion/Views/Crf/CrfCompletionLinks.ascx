<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>

<div id="case-overview">
	<h2>
		Links</h2>
	<%=Html.ActionLink(string.Format("{0} Details",Model.Crf.WorkItemType.GetDescription()),"Details", new {id=Model.Crf.Id}) %>
</div>
