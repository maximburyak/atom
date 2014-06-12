<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>

<% using (Html.BeginForm("Complete", "Crf", FormMethod.Post, new { enctype = "multipart/form-data", id = "addform" }))
   {%>
<div>
	<h2>
		Completion Details</h2>
</div>
<div>
	<p>
		Please enter estimates. These are required before any work can be completed.</p>
</div>
<div>
	<label for="EstimatedUnitsOfWork">
		Estimated Time<br />
		(1 per 15 mins)</label>
	<%=Html.TextBox("EstimatedUnitsOfWork", (Model.Crf.EstimatedUnitsOfWork == null ? "" : Model.Crf.EstimatedUnitsOfWork.ToString()), new { style = "width:100px" })%>
	<%=Html.ValidationMessage("EstimatedUnitsOfWork")%>
</div>
<br />
<div>
	<label for="EstimatedStartDate">
		Estimated Start Date</label>
	<%=Html.TextBox("EstimatedStartDate", Model.Crf.EstimatedStartDate == null ? "" : ((DateTime)Model.Crf.EstimatedStartDate).ToString("dd/MM/yyyy"), new { style = "width:100px" })%>
	<%=Html.ValidationMessage("EstimatedStartDate")%>
</div>
<div>
	<button id="updatedetails" type="submit">
		Update Details</button>
</div>

<%} %>