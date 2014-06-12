<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsViewModel>" %>

<div class="box_grid">
	<div class="crf_grid">
		<div class="case-number priority<%=(int) Model.Crf.Severity%>" data="crf">
			<span title="Open Document">
				<%=Model.Crf.Id%></span>
		</div>
	</div>
	<div class="status_grid">
	<div class="case-status<%=(int)Model.Crf.CrfStatus %>">
			<%=Model.Crf.CrfStatus.GetDescription()%></div>
	</div>
</div>
	<div class="summary_grid">
		<h1>
			<%=Model.Crf.Summary%></h1>
		<span class="case-raised"><b>Raised by:</b>
			<%=Model.Crf.CreatedBy.Name%><br />
			<%=Model.Crf.CreateDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Crf.CreateDate.FormatDateTimeRelative()%>)</span>
		<%if (Model.Crf.AlteredDate.HasValue)
{%>
		<span class="case-updated"><b>Last updated:</b>
			<%=Model.Crf.AlteredDate.Value.FormatDateTimeFull()%><br />(<%=Model.Crf.AlteredDate.Value.FormatDateTimeRelative()%>)</span>
		<%
			}%>
	</div>
	<div class="clear"></div>
	<div class="DescriptionOfChange">
		<h2>Description of Change</h2>
		<div class="commentwrap">
			<div class="header">
				Comment by
				<%=Model.Crf.CreatedBy.Name%>
				<%=Model.Crf.CreateDate.Value.FormatDateTimeRelative()%>
			</div>
			<div class="text">
				<%=Model.Crf.Comments.OrderBy(x=>x.CreateDate).First().CommentText%>
			</div>
		</div>
		<div class="clear">
		</div>
	</div>
	<div class="clear">
	</div>

