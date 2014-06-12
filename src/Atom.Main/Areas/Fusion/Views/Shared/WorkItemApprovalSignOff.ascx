<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>

<%if (Model.WorkItem.WorkItemType == Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Project)
  {
	  Html.RenderPartial("WorkItemChangeBoardSignOff");
  }
  //Otherwise CRF specific sign-off requirements.
  else
  {
	  Html.RenderPartial("CrfSignOff");
  } 
%>