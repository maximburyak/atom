<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%
	if (Model.WorkItem.WorkItemType != Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Incident)
	{
		Html.RenderPartial("WorkItemApprove");
	}
	if (Model.CanPerformTasks())
	{%>
<div id="workitem-tasks">
	<h2>
		Tasks</h2>
	<%
		Html.RenderPartial("WorkItemAssign");
        
		if (Model.WorkItem.WorkStatus <= Atom.Areas.Fusion.Domain.Models.WorkItemStatus.OnHold)
		{
			Html.RenderPartial("WorkItemOnHold");
		}

		if (Model.WorkItem.WorkStatus >= Atom.Areas.Fusion.Domain.Models.WorkItemStatus.InProgress)
		{
			Html.RenderPartial("WorkItemComplete");
		}%>
	<br />
	<div class="clear">
	</div>
</div>
<%}%>
