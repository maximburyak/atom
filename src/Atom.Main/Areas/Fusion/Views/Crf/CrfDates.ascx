<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>
<div id="case-overview">
	<h2>
		Dates</h2>
	<ul>
		<li>
			<h3>
				Crf Received</h3>
			<span>
				<%=Model.Crf.CreateDate.Value.ToString()%></span> </li>
		<li>
			<h3>
				Requested Completion</h3>
			<span>
				<%=Model.Crf.RequestedCompletionDate.ToString()%></span> </li>
			<li>
			<h3>
				Client Requirement</h3>
			<span>
				<%=Model.Crf.ClientRequirement%></span> </li>	
				<li>
			<h3>
				House Keeping</h3>
			<span>
				<%=Model.Crf.IsHouseKeeping%></span> </li>	
		<li>
			<h3>
				Estimated Units of Work</h3>
			<span>
				<%=Model.Crf.EstimatedUnitsOfWork == null? "Not Set" : Model.Crf.EstimatedUnitsOfWork.ToString()%></span>
		</li>
		<li>
			<h3>
				Estimated Start</h3>
			<span>
				<%=Model.Crf.EstimatedStartDate == null ? "Not Set" : Model.Crf.EstimatedStartDate.ToString()%></span>
		</li>
		<%if (Model.Crf.CrfStatus == Atom.Areas.Fusion.Domain.Models.CrfStatus.Completed)
	{ %>
		<li>
			<h3>
				Actual Units of Work</h3>
			<span>
				<%=Model.Crf.ActualUnitsOfWork == null ? "Not Set" : Model.Crf.ActualUnitsOfWork.ToString()%></span>
		</li>
		<%} %>
	</ul>
</div>
