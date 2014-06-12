<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.ProjectDetailsViewModel>" %>

<div class="box_grid">
	<div class="crf_grid">
		<div class="case-number priority<%=(int) Model.Project.Severity%>" data="project">
			<span title="Open Document">
				<%=Model.Project.Id%></span>
		</div>
	</div>
	<div class="status_grid">
	<div class="case-status<%=(int)Model.Project.Status %>">
			<%=Model.Project.Status.GetDescription()%></div>
	</div>
</div>
	<div class="summary_grid">
		<h1>
			<%=Model.Project.Summary%></h1>
		<span class="case-raised"><b>Raised by:</b>
			<%=Model.Project.CreatedBy.Name%><br />
			<%=Model.Project.CreateDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Project.CreateDate.FormatDateTimeRelative()%>)</span>
		<%if (Model.Project.AlteredDate.HasValue)
{%>
		<span class="case-updated"><b>Last updated:</b>
			<%=Model.Project.AlteredDate.Value.FormatDateTimeFull()%><br />(<%=Model.Project.AlteredDate.Value.FormatDateTimeRelative()%>)</span>
		<%
			}%>
	</div>
	<div class="clear"></div>
	<div class="DescriptionOfChange">
		<h2>Description of Change</h2>
		<div class="commentwrap">
			<div class="header">
				Comment by
				<%=Model.Project.CreatedBy.Name%>
				<%=Model.Project.CreateDate.Value.FormatDateTimeRelative()%>
			</div>
			<div class="text">
				<%=Model.Project.Comments.OrderBy(x => x.CreateDate).First().CommentText%>
			</div>
		</div>
		<div class="clear">
		</div>
	</div>
	<div class="clear">
	</div>

