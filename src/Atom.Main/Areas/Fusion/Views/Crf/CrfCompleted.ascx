<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>

<form>
<div class="form">
	<%if (!RoleAuthorizationService.WorkItemCanSeeComplete(Model.WorkItem.WorkItemType))
   {
	%><p style="color: Red;">
		Unfortunately you do not currently have access to see the Completed CRF details, please discuss any comments or concerns you may have with your line manager.</p>
	<%
		}
   else
   {%>
	<div class="crf-complete">
		<div>
			<h2>
				Completed CRF Details</h2>
		</div>
		<div>
			<label for="EstimatedUnitsOfWork">
				Estimated Time</label>
			<%=Html.TextBox("EstimatedUnitsOfWork", (Model.Crf.EstimatedUnitsOfWork == 0 ? "" : Model.Crf.EstimatedUnitsOfWork.ToString()), new { style = "width:100px", @readonly="readonly" })%>
			<%=Html.ValidationMessage("EstimatedUnitsOfWork")%>
		</div>
		<div>
			<label for="EstimatedStartDate">
				Estimated Start Date</label>
			<%=Html.TextBox("EstimatedStartDate", Model.Crf.EstimatedStartDate == null ? "" : ((DateTime)Model.Crf.EstimatedStartDate).ToString("dd/MM/yyyy"), new { style = "width:100px", @readonly="readonly" })%>
			<%=Html.ValidationMessage("EstimatedStartDate")%>
		</div>
		<div>
			<label for="ActualUnitsOfWork">
				Actual Time</label>
			<%=Html.TextBox("ActualUnitsOfWork", (Model.Crf.ActualUnitsOfWork == 0 ? "" : Model.Crf.ActualUnitsOfWork.ToString()), new { style = "width:100px", @readonly = "readonly" })%>
			<%=Html.ValidationMessage("ActualUnitsOfWork")%>
		</div>
		<div>
			<label for="ImpactAnalysis">
				Impact Analysis</label>
			<textarea readonly="readonly" id="ImpactAnalysis" name="ImpactAnalysis" style="width: 350px;"
				rows="8" cols="5"><%=Model.Crf.ImpactAnalysis ?? ""%></textarea>
			<%=Html.ValidationMessage("ImpactAnalysis")%>
		</div>
		<div>
			<label for="CompletionComment">
				Completion Comments</label>
			<input readonly="readonly" type="text" maxlength="255" name="CompletionComment" id="CompletionComment"
				style="width: 350px; height: 100px; overflow: scroll;" value="<%=Model.Crf.CompletionComment ?? ""%>" />
			<%=Html.ValidationMessage("CompletionComment")%>
		</div>
	</div>
</div>
<% } //End Role Check for Seeing Completion Details%>
<br />
<%Html.RenderPartial("CrfSignatures"); %>
<br />
</form>
