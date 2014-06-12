<%@ Page Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.SearchViewModel>" %>
<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Search
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadInclude" runat="server">
	<link href="<%=Url.Stylesheet("case.css") %>" rel="stylesheet" type="text/css" />
	

	<script type="text/javascript">
		var onscrollVar = true;
		var InitSearch = '<%=Model.search%>';
		var modelSearch = '<%=Server.UrlEncode(Model.search)%>';
		var modelAction = '<%=Model.action %>';

		var time = null
		function refreshpage() {
			if($('#searchbox').val()!='' && modelAction=='Query')
				document.forms(0).submit();
			else
				document.location.reload();
		}
		
		<%if (Request.IsAuthenticated && Model.User.Profile != null)
		{
	   if (Model.User.Profile.RefreshSearch)
	   { %>
		$(function() {
			timer = setTimeout('refreshpage()', 30000);
		});
		<%}
	   }%>
		
	</script>

	<script src="<%=Url.JavascriptFusion("workitem/workitem-search.js") %>" type="text/javascript"></script>

	<script id="casetemplate" type="text/html">
	 <# for(var i=0; i < cases.length; i++)     
		{         
			var c = cases[i]; #>
			<div class="caselistitem">
				<div class="case-shadow">
					<div id="<#= c.Id #>" rel="<#= c.Priority#>" class="workitempriority<#= c.Priority #> <#=c.Status#>" href="/Fusion/<#= c.Controller#>/Details/<#= c.Id #>">
						<img class="workitemtypeimage<#=c.WorkItemType#>" src="/Areas/Fusion/Content/Images/icons/<#=c.OnHoldImage#>" alt="<#=c.WorkItemType#>"/>
						<img class=<#if (c.HouseKeeping !="blank_1.png"){#>"housekeepingimage"<#}else{#>"blankimage"<#}#> src="/Areas/Fusion/Content/Images/icons/<#=c.HouseKeeping#>" alt="HouseKeeping"/>
						<img class=<#if (c.ClientRequirement !="blank_1.png"){#>"clientrequirementimage"<#}else{#>"blankimage"<#}#> src="/Areas/Fusion/Content/Images/icons/<#=c.ClientRequirement#>" alt="ClientRequirement"/>
						<img class=<#if (c.InternalTesting !="blank_1.png"){#>"internaltestingimage"<#}else{#>"blankimage"<#}#> src="/Areas/Fusion/Content/Images/icons/<#=c.InternalTesting#>" alt="InternalTesting"/>
							<div class="grid_2 case-number" title="<#=c.WorkItemIndividualStatus#>">
								<div class="case-number priority<#= c.Priority #>"><span><#= c.Id #></span></div>
							</div>
						
						<div class="grid_1">
							<div class="avatar caselistitem">
								<img src="/Areas/Fusion/Content/Images/avatars/<#= c.AssignAvatar #>" alt="<#= c.AvatarAltText#>" title="<#= c.AvatarAltText#>" width="50" height="50" />
							</div>
						</div>
						<div class="grid_11 case-details">
							<div class="case-summary"><#=c.Summary#>
							
							</div>
							
							<div class="case-footer">
								<div class="case-date">
								<#if(c.Status!="Closed") {#>
									Raised <img src="/Areas/Fusion/Content/Images/icons/<#= c.DayOfWeekRaised#>.png" alt="<#= c.DayOfWeekRaisedFull#>" title="<#= c.DayOfWeekRaisedFull#>" width="13" height="13" />
									<#=c.CreateDate#> by <span><#=c.CreatedBy#></span><#if (c.LastUpdatedRelative !=""){#>, Updated <a style="color:#666666" title="<#= c.LastUpdatedFull#>"><#= c.LastUpdatedRelative#></a> by <span><#=c.AlteredBy#></span><#} #>
								<#}else {#>
									Closed <#=c.ClosedDate#> by <span><#=c.ClosedBy#></span><#if (c.AssignedToName !="") {#>, Assigned to <#= c.AssignedToName#><#} #>
								<#}#>
								</div>
								<div class="case-area">
									<#if(c.Area=="") {#>
										<#=c.ProgressStatus#>
									<#}else {#>
										<#=c.Area#>
									<#}#>
								</div>
							</div>
						</div>
						<div class="clear"></div>
					</div>
				</div>
			</div>
	 <# } #>
	</script>

</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container_16">
		<%Html.RenderPartial("MyFilters"); %>
	</div>
	<br />
	<div class="container_16 caselist" id="caselist">
	</div>
	<div class="container_16">
		<div id="end-results" style="display: none">
			<div id="end-results-title">
				<h1>
					End of results - no more to show</h1>
			</div>
		</div>
	</div>
</asp:Content>
