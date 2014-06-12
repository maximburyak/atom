<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>
<% using (Html.BeginForm("Complete", "Crf", FormMethod.Post, new { enctype = "multipart/form-data", id = "addform" }))
   {%>
<div>
	<h2>
		Completion Details</h2>
</div>

<div>
	<label for="EstimatedUnitsOfWork">
		Estimated Time<br />
		(1 per 15 mins)</label>
	<%=Html.TextBox("EstimatedUnitsOfWork", (Model.Crf.EstimatedUnitsOfWork.Value.ToString()), new { style = "width:100px;", disabled="disabled" })%>
	<%=Html.ValidationMessage("EstimatedUnitsOfWork")%>
</div>
<br />
<div>
	<label for="EstimatedStartDate">
		Estimated Start Date</label>
	<%=Html.TextBox("EstimatedStartDate", Model.Crf.EstimatedStartDate.Value.ToString("dd/MM/yyyy"), new { style = "width:100px", disabled = "disabled" })%>
	<%=Html.ValidationMessage("EstimatedStartDate")%>
</div>
<div>
	<p style="color:red">
		Please enter actual time spent, impact analysis and any relevant completion comments.</p>
</div>
<br />
<div>
	<label for="ActualUnitsOfWork">
		Actual Time<br />
		(1 per 15 mins)</label>
	<%=Html.TextBox("ActualUnitsOfWork", (Model.Crf.ActualUnitsOfWork == null ? "" : Model.Crf.ActualUnitsOfWork.ToString()), new { style = "width:100px" })%>
	<%=Html.ValidationMessage("ActualUnitsOfWork")%>
</div>
<br />
<div>
	<label for="ImpactAnalysis">
		Impact Analysis<br />
		Min 25, Max 2000 chars</label>
	<textarea id="ImpactAnalysis" name="ImpactAnalysis" style="width: 350px;" rows="8"
		cols="5"><%=Model.Crf.ImpactAnalysis ?? ""%></textarea>
	<%=Html.ValidationMessage("ImpactAnalysis")%>
</div>
<br />
<div>
	<label for="CompletionComment">
		Completion Comments<br />
		Max 2000 chars</label>
	<textarea id="CompletionComment" name="CompletionComment" style="width: 350px;" rows="8"
		cols="5"><%=Model.Crf.CompletionComment ?? ""%></textarea>
	<%=Html.ValidationMessage("CompletionComment")%>
</div>
<div>
	<button id="updatedetails" type="submit">
		Update Details</button>
</div>

<%} %>