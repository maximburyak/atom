<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%
	// Retrieve SignOff ID for form.
	var signOff = Model.WorkItem.SignOffs.Where(x => x.SignOffType == Atom.Areas.Fusion.Domain.Models.SignOffTypeEnum.ChangeBoardAcceptance).First();
%>
<form method="post" id="workitem-approve-form" action="/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription()%>/SignOff/<%=signOff.Id%>">
<div id="severitylist">
	<%if (Model.WorkItem.WorkStatus == Atom.Areas.Fusion.Domain.Models.WorkItemStatus.AwaitingApproval && !Model.WorkItemHasBeenApproved())
   {%>
	<h3 style="padding-bottom: 5px;">
		Update Severity?</h3>
	<%=Html.DropDownList("Severity",new SelectList(Model.Severity(), "Key", "Value",((int) Model.WorkItem.Severity)), new {style = "width:200px;"})%>
	<%}%>
</div>
<div id="signaturelist">
	<%=Html.WorkItemChangeBoardSignOff(Model.WorkItem, Model.User)%>
</div>
</form>
