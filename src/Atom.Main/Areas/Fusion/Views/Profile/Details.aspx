<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Profile Details
	</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
	
	<script type="text/javascript" src="<%=Url.Javascript("jquery.validate.js") %>"></script>

	<script type="text/javascript" src="<%= Url.JavascriptFusion("jquery.MultiFile.js") %>"></script>

	<script type="text/javascript" src="<%= Url.JavascriptFusion("profile/profile-details.js") %>"></script>
	
	<script type="text/javascript" src="<%=Url.JavascriptFusion("xVal.jquery.validate.js") %>" ></script>
	
	<script type="text/javascript">  			
			$(function() {
			<% var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			   Html.ClientSideValidation("", RuleSet.Empty).SuppressScriptTags()
				   .AddRule("ChangeBoardMeetingDate", new RangeRule(now, now.AddMonths(6))); %>
					
			});	
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container_12">
		<div class="grid_10 prefix_1">
			<div class="form">
				<fieldset>
					<legend>Profile</legend>
					<%Html.RenderPartial("ProfileAvatars"); %>
					<%Html.RenderPartial("ProfileSignatures"); %>
					<%Html.RenderPartial("ProfileFilters"); %>
					<%Html.RenderPartial("ProfileSubscriptions"); %>
					<%Html.RenderPartial("ProfileCAB"); %>
					<%Html.RenderPartial("ProfileAutoAssignTo"); %>
				</fieldset>
			</div>
		</div>
	</div>
</asp:Content>
