<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.AddProjectViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Add New Project
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
	<%if (RoleAuthorizationService.ProjectCreateView())
   {
	%>
	<script src="<%=Url.Javascript("jquery.validate.js") %>" type="text/javascript"></script>
	<script src="<%=Url.JavascriptFusion("jquery.MultiFile.js")%>" type="text/javascript"></script>
	<script src="<%=Url.JavascriptFusion("xVal.jquery.validate.js") %>" type="text/javascript"></script>
	<%Html.RenderPartial("WMD-Includes");%>
	<script type="text/javascript">
		
		$(function() {
		<%
			var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); %>
		<%=Html.ClientSideValidation("",RuleSet.Empty).SuppressScriptTags()
		.AddRule("Summary", new RequiredRule())
		.AddRule("wmd-input", new RequiredRule()) 
		.AddRule("Severity", new RequiredRule()) 
		.AddRule("RequestedCompletionDate", new RangeRule(now,now.AddMonths(6)))
		.AddRule("wmd-input", new RegularExpressionRule("[^\t]{50,}",RegexOptions.IgnoreCase))
		%>

			$('#fileupload').MultiFile({
					max: 1,
					accept: 'gif,jpg,png,jpeg,doc,docx,pdf,xls,xlsx,ppt,pps,vsd,mpp,msg'
			});			

			$("#createproject").click(function() {
				window.onbeforeunload = null; 
				$(this).attr("disabled",true);
				$("#addform").submit();
			});
			
			$("#addform").bind('change keydown', function() {
				window.onbeforeunload = function() { return "You have chosen to cancel the creation of a Project. Any completed information will be lost. Are you sure?"; }
				$("#createcase").attr("disabled",false);
			});

		});
		
	</script>
	<%
		}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container_12">
		<div class="grid_10 prefix_1">
			<div class="form" id="case-add">
				<%Html.RenderPartial("ProjectAdd");%>
			</div>
		</div>
	</div>
</asp:Content>
