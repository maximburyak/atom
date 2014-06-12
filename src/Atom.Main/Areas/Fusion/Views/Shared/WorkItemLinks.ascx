<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<script type="text/javascript">
	var url = '<%= Url.Action("SearchLinks","Search") %>';
	var workitemidtolinkto = <%= Model.WorkItem.Id %>;
</script>
<script src="<%=Url.JavascriptFusion("workitem/workitem-linked.js") %>" type="text/javascript"></script>
<div id="case-links">
	<%if (Request.IsAuthenticated)
   { %>
	<%if (Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
   {%>
	<%if (RoleAuthorizationService.WorkItemChangeLinks(Model.WorkItem.WorkItemType))
   {%>
	<h2>
		Linked Items</h2>
	<% using (Html.BeginForm("UnLinkWorkItems/" + Model.WorkItem.Id, Model.WorkItem.WorkItemType.GetDescription(), FormMethod.Post, new { id = "linkedworkitems-form" }))
	{ %>
	<div id="casesearchlist" class="linkedworkitemlist">
		
			<%=Html.LinkedItems(Model.LinkedWorkItems) %>
		
		<%if (Model.LinkedWorkItems.Any())
	{ %>
	
	<br/>
		<input id="saveUnLinkedItems" disabled="disabled" type="button" value="Unlink" style="width: 90px; display: inline;" />
		<%}
	} %>
	</div>
	<br />
	<h3>
		Search Items</h3>
	<div id="linksearchcontainer">
		<div id="linksearchboxwrap">
			<input type="text" autocomplete="off" name="linksearchbox" id="linksearchbox" />
			<img src="/Areas/Fusion/Content/Images/icons/searchbox.png" onclick="GetLinkResults();"
				alt="Search" style="cursor: pointer" />
		</div>
		<div id="detailscontainer" style="display: none">
			<h3 style="margin-top: 5px;">
				Results:</h3>
			<br />
			<div id="details-results">
			</div>
		</div>
	</div>
	<%}
   else
   { %>
	<p style="color: Red;">
		You do not have the required role to manage links</p>
	<%} %>
	<%}%>
	<%}
   else
   { %>
	<p style="color: Red;">
		You do not have the required role to manage links</p>
	<%} //End Request Authenticated if %>
</div>
