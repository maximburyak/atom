<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.CrfDetailsBaseViewModel>" %>

<%if (!RoleAuthorizationService.WorkItemCanSeeSignatures(Model.WorkItem.WorkItemType))
  {
	 %><p style="color:Red;">YOu do not have the required role to see signatures.</p><%
  }
  else
  {%>
<br />
<div class="crf-complete">
	<h2>
		Signatures</h2>
	<p>
		Set signatures for the CRF</p>
	<div id="signaturelist">
		<%=Html.RequiredSignOffList(Model.User,Model.Crf.SignOffs,Model.Crf) %>
	</div>
</div>

<%} //End Role check for seeing signature details%>

