<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>

<div id="workitem-approve">
	<h2>
		Change Board Approval</h2>
	<%if (Request.IsAuthenticated)
   {%>
	<div id="workitem-approval-content">
		<!-- Show Emergency Change (CRF only) -->
		<%Html.RenderPartial("CrfEmergencyChange"); %>
		<br />
		<!-- Show Type of Change Sign-off (Change Board or Emergency) -->
		<%Html.RenderPartial("WorkItemApprovalSignOff"); %>
		
		<!-- Show Rejection Options where applicable -->
		<%if (Model.WorkItem.WorkStatus <= Atom.Areas.Fusion.Domain.Models.WorkItemStatus.OnHold)
			{ 
			Html.RenderPartial("WorkItemReject"); 
	} 
		}  // end request authenticated check%>
		<div class="clear">
		</div>
	</div>
</div>