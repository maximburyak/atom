<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.SearchViewModel>" %>

	<script type="text/javascript">
		var onscrollVar = true;
		var InitSearch = '<%=Model.search %>';
		var modelSearch = '<%=Server.UrlEncode(Model.search) %>';
		var modelAction = '<%=Model.action %>';
		var currentWorkItemId = '<%=Model.workItemIdToLinkTo %>';
	</script>

	<script src="<%=Url.JavascriptFusion("workitem/workitem-links-search.js") %>" type="text/javascript"></script>

	<script id="casetemplate" type="text/html">
	 <# for(var i=0; i < cases.length; i++)     
		{         
			var c = cases[i]; #>
				<div style="height:25px;" class="caseitem" id="caseitem-<#=c.Id#>">
					<div style="width:200px;">
						<span style="float:left;">
							<a title="<#= c.Summary#>" target="_blank" href="/Fusion/<#=c.Controller #>/Details/<#= c.Id #>"><#= c.Id #> (<#= c.Controller #>)</a>
						</span>
						<span style="float:right;">
							<input class="chk" style="width:20px" name="selectedWorkItems" id="<#= c.Id #>" value="<#= c.Id #>" type="checkbox" />
						</span>
					</div>
				</div>
				<div class="clear"></div>
			
	 <# } #>
	</script>
	<%using (Html.BeginForm("LinkWorkItems/" + Model.workItemIdToLinkTo, Model.WorkItemType.GetDescription(), FormMethod.Post, new { id = "linkworkitems-form" }))
	  {%>
		<div id="caselist">
			<!--template goes here -->   
		</div>
		<div id="save-links" class="container_16">
			<input disabled="disabled" id="saveLinks" type="button" value="Save Links" style="width: 90px; display: inline;" />
		</div>
		<div class="container_16">
			<div id="end-results" style="display: none">
				<div>
					No results to show
				</div>
			</div>
		</div> 
	   
	<%} %>