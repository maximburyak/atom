<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>
<% 
	if (RoleAuthorizationService.ProfileViewSubscriptions())
	{
%>
<hr style="color: #CCCCCC" />
<div class="subscription">
	<img alt="Subscriptions" id="subscriptions" src="/Areas/Fusion/Content/Images/icons/subscriptions.png" />
	<h1>
		Your Work-Item Subscriptions</h1>
	<div>
		<ul class="subscription-list">
			<%=Html.SubscriptionList(Model.Subscriptions) %>
		</ul>
	</div>
	<br />
</div>
<%
	}//End Role Check%>
