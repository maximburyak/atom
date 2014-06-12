<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (ViewData.Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
  {
	  if (Request.IsAuthenticated)
	  {
		  if (RoleAuthorizationService.WorkItemAddComment(Model.WorkItem.WorkItemType))
		  {%>
<button id="addcomment" type="submit">
	Add Comment</button>
&nbsp; <span>
	<%if (Model.WorkItem.WorkItemType == Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Incident && RoleAuthorizationService.IncidentClosure())
   { %>
	<span>
		<button id="closecase">
			Add Comment &amp; Close</button>
	</span>
	<%} //end incident check%>
	<%
		} //end role check
	} // end Auth check
 }//end closed item check%>