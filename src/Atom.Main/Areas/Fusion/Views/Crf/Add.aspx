<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.AddCrfViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Add New Change Request
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">

<%if (RoleAuthorizationService.CrfCreateView())
 {
%>

	<script src="<%=Url.Javascript("jquery.validate.js") %>" type="text/javascript"></script>

	<script src="<%=Url.JavascriptFusion("jquery.MultiFile.js")%>" type="text/javascript"></script>

	<script src="<%=Url.JavascriptFusion("xVal.jquery.validate.js") %>" type="text/javascript"></script>

	<script src="<%=Url.JavascriptFusion("crf/crf-add.js") %>" type="text/javascript"></script>

	<%Html.RenderPartial("WMD-Includes");%>

	<script type="text/javascript">
		var cbDate = '<%=Model.ChangeBoardMeetingDate.ToString("dd/MM/yyyy")%>';

		$(function() {
			<%
			var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); %>
			<%=Html.ClientSideValidation("",RuleSet.Empty).SuppressScriptTags()
			.AddRule("Summary", new RequiredRule())
			.AddRule("wmd-input", new RequiredRule()) 
			.AddRule("Alternatives", new RequiredRule())
			.AddRule("Severity", new RequiredRule())
			.AddRule("BusinessBenefit", new RequiredRule())
			.AddRule("Channels", new RequiredRule())
			.AddRule("InsuranceCompanies", new RequiredRule())
			.AddRule("emergencychange", new RequiredRule())
			.AddRule("Suppliers", new RequiredRule())
			.AddRule("RequestedCompletionDate", new RangeRule(now,now.AddMonths(6)))
			.AddRule("RequestedCompletionDate", new CustomRule("changeBoardMeeting",new {},"You must set this as an Emergency Change..."))
			.AddRule("wmd-input", new RegularExpressionRule("[^\t]{50,}",RegexOptions.IgnoreCase))
			.AddRule("BusinessBenefit", new StringLengthRule(50,2000))
			.AddRule("ProductGroups", new RequiredRule())%>

			$('#fileupload').MultiFile({
					max: 1,
					accept: 'gif,jpg,png,jpeg,doc,docx,pdf,gif,xls,xlsx,ppt,pps,vsd,mpp,msg,xml,txt,csv,zip'
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
				<%Html.RenderPartial("CrfAdd"); %>
			</div>
		</div>
	</div>
</asp:Content>
