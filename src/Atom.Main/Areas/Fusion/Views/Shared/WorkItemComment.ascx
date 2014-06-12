<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (ViewData.Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
  {
	  if (Request.IsAuthenticated)
	  {
		  if (RoleAuthorizationService.WorkItemAddComment(Model.WorkItem.WorkItemType))
		  {%>
<form id="frmaddc" method="post" action="/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/AddComment/<%=Model.WorkItem.Id%>">
<div class="workitem-comment">
	<div id="wmd-editor" class="wmd-panel">
		<div id="wmd-button-bar">
		</div>
		<textarea style="width: 650px;" id="wmd-input" name="CommentText"></textarea>
		<div id="wmd-preview" class="wmd-panel">
		</div>
		<div>
			<%if (RoleAuthorizationService.UnitsOfWorkRequired(Model.WorkItem.WorkItemType))
	 {%>
			<label for="UnitsOfWork">
				Enter Units of Work (mins)
			</label>
			<%=Html.TextBox("UnitsOfWork", "", new { style = "width:100px", maxlength = "6" })%>
			<%}
	 else
	 {%>
			<%=Html.Hidden("Comments[0].UnitsOfWork", "0")%>
			<%=Html.Hidden("UnitsOfWork", "0")%>
			<%} %>
			<br />
			<%if (Model.WorkItem.WorkItemType == Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Incident)
	 {
		 Html.RenderPartial("WorkItemCommentAssignTo");
	 }%>
			<%
				Html.RenderPartial("WorkItemCommentExtraActions"); %>
		</div>
	</div>
</div>
</form>
<%} //end role check
		  else
		  {%>
<p style="color: Red;">
	You do not have the required role to add a comment</p>
<%}
	  } //end logged in check
	  else
	  {%>
<%=Html.ActionLink("Add Comment", "AddComment", Model.WorkItem.WorkItemType.GetDescription() , new {id = Model.WorkItem.Id}, null)%>
<%
	}
  }%>