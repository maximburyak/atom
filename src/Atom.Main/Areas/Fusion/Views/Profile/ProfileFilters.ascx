<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>
<% 
	if (RoleAuthorizationService.ProfileViewSubscriptions())
	{
%>
<hr style="color: #CCCCCC" />
<div class="profile-filters">
	<img alt="Filters" id="filters" src="/Areas/Fusion/Content/Images/icons/filters.png" />
	<h1>
		Your Filters</h1>
	<div>
		<%=Html.ProfileFilterButton(Model.User)%>
	</div>
	<br />
	<div id="profile-filters-list">
		<span class="profile-filters-list-span">
			<% using (Html.BeginForm("RemoveFilter", "Profile", FormMethod.Post, new { id = "profilefilters", style = "display:inline;" }))
	  {%>

	  <select id="filterid" name="filterid">
				<option value="">Please select Filter</option>
				<%foreach (var d in Model.Filters)
	  {%>
				<option value="<%=d.FilterValue %>">
					<%=d.DisplayText() %></option>
				<%} %>
			</select>
		
			<%=Html.FilterDeleteImage()%>
			<%} %>
			<% using (Html.BeginForm("FilterDefault", "Profile", FormMethod.Post, new { id = "profiledefault", style = "display:inline;" }))
	  {
			%>
			<%=Html.Hidden("filterdefault") %>
			<%=Html.FilterDefaultImage()%>
			<%} %>
			<% using (Html.BeginForm("RemoveFilterDefault", "Profile", FormMethod.Post, new { id = "profiledefaultremove", style = "display:inline;" }))
	  {%>
			<%=Html.FilterDefaultDeleteImage()%>
			<%=Html.Hidden("filterremove")%>
			<%}%>
		</span>
	</div>
	<br />
</div>
<%
	}//End Role Check%>
