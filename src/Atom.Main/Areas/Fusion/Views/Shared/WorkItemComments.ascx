<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%
	var i = 1;
	var skipNumber = (Model.WorkItem.WorkItemType == Atom.Areas.Fusion.Domain.Models.WorkItemTypeEnum.Crf) ? 1 : 0;

	if (Model.WorkItem.Comments.Any())
	{%>
<h2>
	Progress Notes</h2>
<%
	foreach (var c in Model.WorkItem.Comments.OrderByDescending(x => x.CreateDate.Value).Take(Model.WorkItem.Comments.Count - skipNumber))
	{
		i++;%>
<div class="cmt" id="CommentText_<%=i%>">
	<%=Html.Avatar(c.CreatedBy, new {@class = "avatar"})%>
	<div class="commentwrap">
		<div class="header">
			Comment by
			<%=c.CreatedBy.Name%>
			<%=c.CreateDate.Value.FormatDateTimeRelative()%>, (<%=c.UnitsOfWork%>
			Units of Work)
		</div>
		<div class="text">
			<%=c.CommentText%>
		</div>
	</div>
	<div class="clear">
	</div>
</div>
<%
}
	}
%>