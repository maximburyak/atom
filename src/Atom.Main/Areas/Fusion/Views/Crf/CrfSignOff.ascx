<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (Model.WorkItemChangeTypeIsKnown())
  {
if (!Model.WorkItemIsEmergencyChange())
  {
	  Html.RenderPartial("WorkItemChangeBoardSignOff");
  }
  else
  { %>
<div id="signaturelist">
	<%=Html.WorkItemEmergencySignOff(Model.WorkItem,Model.User) %>
</div>
<%}
  }%>