<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Change Request Details:
	<%=Model.Crf.Id %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
	<link href="<%=Url.Stylesheet("case.css") %>" rel="stylesheet" type="text/css" />

	<script src="<%= Url.JavascriptFusion("jquery.MultiFile.js")%>" type="text/javascript"></script>
	<script src="<%=Url.JavascriptFusion("workitem/workitem-common.js") %>" type="text/javascript"></script>

	<script type="text/javascript">
	var CommentAdded = <%=Model.CommentAdded.ToString().ToLower() %>;
	var UnitsOfWorkRequired = <%=RoleAuthorizationService.UnitsOfWorkRequired(Model.WorkItem.WorkItemType).ToString().ToLower() %>;
	</script>

	<script src="<%=Url.JavascriptFusion("workitem/workitem-details-common.js") %>" type="text/javascript"></script>

	<%Html.RenderPartial("WMD-Includes"); %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="case-details">
		<div class="container_16">
			<div class="grid_2">
				<div class="case-number priority<%=(int) Model.Crf.Severity%>">
					<span>
						<%=Model.Crf.Id%></span>
				</div>
			</div>
			<div class="grid_2">
				<div class="case-status<%=(int)Model.Crf.CrfStatus %>">
					<%=Model.Crf.CrfStatus.GetDescription()%></div>
			</div>
			<div class="grid_12">
				<h1>
					<%=Model.Crf.Summary%></h1>
				<span class="case-raised">Raised by:
					<%=Model.Crf.CreatedBy.Name%>
					<%=Model.Crf.CreateDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Crf.CreateDate.FormatDateTimeRelative()%>)</span>
				<%if (Model.Crf.AlteredDate.HasValue)
	  {%>
				<span class="case-updated">Last updated:
					<%=Model.Crf.AlteredDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Crf.AlteredDate.Value.FormatDateTimeRelative()%>)</span>
				<%
					}%>
			</div>
			<div class="clear">
			</div>
		</div>
		<div class="container_16">
			<div class="grid_4">
				<%Html.RenderPartial("WorkItemTasks");%>
				<%Html.RenderPartial("CrfDates");%>
				<%Html.RenderPartial("CrfScopeOfChange"); %>
				<%Html.RenderPartial("WorkItemLinks");%>
				<%Html.RenderPartial("WorkItemDocuments"); %>
				<%Html.RenderPartial("WorkItemSubscriptions"); %>
			</div>
			<div class="grid_12">
				<div class="cmt" id="DescriptionOfChange">
					<h2>
						Description of Change</h2>
					<%=Html.Avatar(Model.Crf.CreatedBy, new { @class = "avatar" })%>
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
				<div class="cmt" id="businessbenefit">
					<h2>
						Business Benefit</h2>
					<%=Html.Avatar(Model.Crf.CreatedBy, new { @class = "avatar" })%>
					<div class="commentwrap">
						<div class="header">
							Comment by
							<%=Model.Crf.CreatedBy.Name%>
							<%=Model.Crf.CreateDate.Value.FormatDateTimeRelative()%>
						</div>
						<div class="text">
							<%=Model.Crf.BusinessBenefit%>
						</div>
					</div>
					<div class="clear">
					</div>
				</div>
				<br />
				<div class="cmt" id="Alternatives">
					<h2>
						Alternatives</h2>
					<%=Html.Avatar(Model.Crf.CreatedBy, new { @class = "avatar" })%>
					<div class="commentwrap">
						<div class="header">
							Comment by
							<%=Model.Crf.CreatedBy.Name%>
							<%=Model.Crf.CreateDate.Value.FormatDateTimeRelative()%>
						</div>
						<div class="text">
							<%=Model.Crf.Alternatives%>
						</div>
					</div>
					<div class="clear">
					</div>
				</div>
				<br />
				<%Html.RenderPartial("WorkItemComments"); %>
				<%if (Model.Crf.CrfStatus != Atom.Areas.Fusion.Domain.Models.CrfStatus.Completed)
	  {%>
				<h2>
					Your Comment</h2>
				<%}%>
				<%Html.RenderPartial("WorkItemComment"); %>
				<%if (Model.Crf.CrfStatus == Atom.Areas.Fusion.Domain.Models.CrfStatus.Completed)
	  { %>
				<%Html.RenderPartial("CrfSignatures"); %>
				<%} %>
			</div>
			<div class="clear">
			</div>
		</div>
	</div>
	<div class="clear">
	</div>
</asp:Content>
