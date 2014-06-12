<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.SearchViewModel>" %>
<%if (Request.IsAuthenticated)
  {

	  if (Model.User.Profile != null)
	  {
		  if (Model.User.Profile.ShowFilters && Model.InHomeScreen())
		  {%>
<div id="filters-results">
	<div id="filters-results-content">
		<div style="margin-bottom: 10px;">
			<h1 id="filters-link">
				My Filters:</h1>
			<select id="filter-list" name="filter-list">
				<option value="">Please select Filter</option>
				<%foreach (var d in Model.filters)
	  {%>
				<option value="<%=d.FilterValue %>" <%if(d.FilterValue==Model.DefaultFilter.FilterValue) {%> selected="selected" <%} %> >
					<%=d.DisplayText() %></option>
				<%} %>
			</select>
			<%//=Html.DropDownList("filter-list", new SelectList(Model, "FilterValue", "Description", Model.DefaultFilter.FilterValue), "Please select Filter")%>
			<span>
				<img id="filters-change" src="/Areas/Fusion/Content/Images/icons/blank.png" /></span>
		</div>
		<div style="display: inline; vertical-align: middle">
			<h1 id="filters-displaytext">
			</h1>
			<a id="filters-savecurrent" href="#" style="display: none;">
				<img src="/Areas/Fusion/Content/Images/icons/filter_Add.png" title="Save Filter"
					alt="Save Filter" /></a>
		</div>
		<div style="display: inline">
			<img id="filter-outcome" src="/Areas/Fusion/Content/Images/icons/blank.png" style="display: none" /></div>
		<div id="filters-description">
			<div id="filters-description-content" style="display: none; width: 700px;">
				<div>
					Please enter a label for your filter:
					<input type="text" maxlength="255" id="filterlabel" value="" style="width: 200px;
						display: inline;" />
					<input type="button" id="filter-save" style="width: 75px; display: inline;" value="Save Filter" />
				</div>
			</div>
		</div>
	</div>
	<input type="hidden" name="searchFilter" id="searchFilter" value="" />
</div>
<%
		  }
		  else
		  {%>
<div style="display: inline; vertical-align: middle">
	<h1 id="filters-displaytext">
	</h1>
</div>
<%}
	  }
  }%>
<div>
	<h1 style="display: none" id="filters-displaycount">
	</h1>
</div>
<hr />
