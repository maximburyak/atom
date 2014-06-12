<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.ProjectDetailsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Project Details
	<%=Model.Project.Id %>
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
				<div class="case-number priority<%=(int) Model.Project.Severity%>">
					<span>
						<%=Model.Project.Id%></span>
				</div>
			</div>
			<div class="grid_2">
				<div class="case-status<%=(int)Model.Project.Status %>">
					<%=Model.Project.Status.GetDescription()%></div>
			</div>
			<div class="grid_12">
				<h1>
					<%=Model.Project.Summary%></h1>
				<span class="case-raised">Raised by:
					<%=Model.Project.CreateDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Project.CreateDate.FormatDateTimeRelative()%>)</span>
				<%if (Model.Project.AlteredDate.HasValue)
	  { %>
				<span class="case-updated">Last updated:
					<%=Model.Project.AlteredDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Project.AlteredDate.Value.FormatDateTimeRelative()%>)</span>
				<%} %>
			</div>
			<div class="clear">
			</div>
		</div>
		<div class="container_16">
			<div class="grid_4">
				<%Html.RenderPartial("WorkItemTasks");%>
				<div id="case-overview">
					<h2>
						Overview</h2>
					<ul>
						<li>
							<h3>
								Client Requirement</h3>
							<%=Model.Project.ClientRequirement%>
						</li>
						<li>
							<h3>
								House Keeping</h3>
							<%=Model.Project.IsHouseKeeping%>
						</li>
					</ul>
				</div>
				<%Html.RenderPartial("WorkItemDocuments"); %>
				<%Html.RenderPartial("WorkItemSubscriptions"); %>
			</div>
			<div class="grid_12">
				<br />
				<%Html.RenderPartial("WorkItemComments"); %>
				<h2>
					Your Comment</h2>
				<%Html.RenderPartial("WorkItemComment"); %>
			</div>
			<div class="clear">
			</div>
		</div>
	</div>
	<div class="clear">
	</div>
</asp:Content>
