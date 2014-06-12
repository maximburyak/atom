<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.CrfCompleteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Complete Change Request:
	<%=Model.Crf.Id %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
	<link href="<%=Url.Stylesheet("case.css") %>" rel="stylesheet" type="text/css" />

	<script src="<%=Url.JavascriptFusion("highlight.pack.js") %>" type="text/javascript"></script>

	<script src="<%=Url.JavascriptFusion("jquery.MultiFile.js")%>" type="text/javascript"></script>

	<script src="<%=Url.Javascript("jquery.validate.js") %>" type="text/javascript"></script>

	<script src="<%=Url.JavascriptFusion("xVal.jquery.validate.js") %>" type="text/javascript"></script>

	<link href="<%=Url.Stylesheet("vs.css") %>" rel="stylesheet" type="text/css" />
	<%if (Model.CanStartWorkOnCrf())
   {
	   Html.RenderPartial("WMD-Includes");
   }%>

	<script type="text/javascript">
		$(function() {
		
			simple_tooltip("a", "tooltip");
		
			<%var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); %>
			<%=Html.ClientSideValidation("",RuleSet.Empty).SuppressScriptTags()
			.AddRule("EstimatedUnitsOfWork", new RequiredRule())
			.AddRule("ActualUnitsOfWork", new RequiredRule())
			.AddRule("EstimatedStartDate", new RequiredRule()) 
			.AddRule("EstimatedStartDate", new RangeRule(now,now.AddMonths(6)))
			.AddRule("ImpactAnalysis", new RequiredRule()) 
			.AddRule("CompletionComment", new RegularExpressionRule("[^\t]{1,2000}",RegexOptions.IgnoreCase))
			.AddRule("ImpactAnalysis", new RegularExpressionRule("[^\t]{25,2000}",RegexOptions.IgnoreCase))%>
		
			hljs.initHighlightingOnLoad();
			
			$('#addcomment').click(function() {    		
				var unitsOfWork = $('#UnitsOfWork').val();
				var msg='';
				<%if(RoleAuthorizationService.UnitsOfWorkRequired(Model.WorkItem.WorkItemType)){%>
					if (unitsOfWork =='') {
						msg = 'Please ensure units of work is completed.';
					}
				<%}%>
				if ($('#wmd-input').val() == '') {
					msg+="\nPlease ensure comment is completed."
				}
				
				if(msg=='') {
					$('#frmaddc').submit();
				}
				else {
					alert(msg);
					return false;
				}
			}); 
			
			$('#UnitsOfWork, #EstimatedUnitsOfWork, #ActualUnitsOfWork').keypress(function(e) {
				var keycode = e.which;
				var keypressed = String.fromCharCode(keycode)
				var value = $(this).val();
				return (keycode > 47 && keycode < 58 || keycode == 8 || keycode == 0);
			});
			
		});
		
		
	</script>

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
					<%=Model.Crf.CreateDate.FormatDateTimeRelative()%></span> <span class="case-updated">
						Last updated:
						<%=Model.Crf.AlteredDate.FormatDateTimeRelative()%></span>
			</div>
			<div class="clear">
			</div>
		</div>
		<div class="container_16">
			<div class="grid_4">
				<br />
				<%Html.RenderPartial("CrfDates");%>
				<%Html.RenderPartial("CrfScopeOfChange"); %>
				<%Html.RenderPartial("CrfCompletionLinks"); %>
			</div>
			<div class="grid_12">
				<%Html.RenderPartial("CrfCompletion"); %>
			</div>
			<div class="clear">
			</div>
</asp:Content>
