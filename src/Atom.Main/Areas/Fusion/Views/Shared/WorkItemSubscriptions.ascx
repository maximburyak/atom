<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>

<div id="case-subscriptions">
	<h2>
		Subscription</h2>
		
<%if (Request.IsAuthenticated){ %>

	<%if (Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed){%>
		  <%if(RoleAuthorizationService.WorkItemChangeSubscription(Model.WorkItem.WorkItemType)) {%>
		
		<%=Html.Avatar(Model.User, new { @class = "avatar",style="vertical-align:middle;" })%>
			<span style="margin-left: 4px; vertical-align: middle;">
				<%=Html.SubscriptionLink(Model.WorkItem,Model.User) %>
			</span>

		<br />

		<%Html.RenderPartial("WorkItemSubscribeOthers"); %>
    
        <% if (Model.SubscribedUsers.Any())
           { %>
               <div style="padding-top: 20px; padding-bottom: 10px;">Subscribed Users:</div>
               <% foreach (var s in Model.SubscribedUsers)
                  { %>
                   <div style="padding-top: 5px;">
                   <%= Html.Avatar(s, new {@class = "avatar", style = "vertical-align:middle;"}) %> <%= s.Name %>
                   </div>
               <% } %>
                
        <% } else { %>
            <div style="padding-top: 10px;">No subscribed users.</div>
        <% } %>

		<%}else { %>
			<p style="color:Red;">You do not have the required role to manage a subscription</p>
		<%} %>
	<%}%>

<%}
  else{ 
%>
	<%=Html.ActionLink("Manage Subscription", "Subscribe", Model.WorkItem.WorkItemType.GetDescription() , new {id = Model.WorkItem.Id}, null)%>
<%} //End Request Authenticated if %>
</div>

