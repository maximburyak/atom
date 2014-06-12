<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>

<div id="case-overview">
	<h2>
		Scope of Change</h2>
	<ul>
		
		<li>
			<h3>
				Insurance Companies</h3>
			<span>
				<%=Html.WorkItemInscos(Model.Crf)%></span> </li>
		<li>
			<h3>
				Suppliers</h3>
			<span>
				<%=Html.WorkItemSuppliers(Model.Crf)%></span> </li>
		<li>
			<h3>
				Product Groups</h3>
			<span>
				<%=Html.WorkItemProductGroups(Model.Crf)%></span> </li>
		<li>
			<h3>
				Other Scope</h3>
			<span>
				<%=Model.Crf.OtherScope%></span> </li>
	</ul>
</div>
