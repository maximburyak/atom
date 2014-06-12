<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<div id="case-documents">
	<h2>
		Documents</h2>
	<%if (Request.IsAuthenticated)
   {%>
	<%if (!RoleAuthorizationService.WorkItemViewDocuments(Model.WorkItem.WorkItemType))
   { %>
	<p style="color: Red;">
		You do not have the required role to see documents</p>
	<%} //End If Roles is !Authed

   else // Else we can see it all (so long as it aint closed)
   {%>
	<ul>
		<%
			if (Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
			{ %>
		<li>
			<h3>
				Add New</h3>
		</li>
		<li>
			<form action="/Fusion/<%=Model.WorkItem.WorkItemType.GetDescription() %>/AddDocument/<%=Model.WorkItem.Id%>"
			method="post" id="adddoc" enctype="multipart/form-data">
			<input type="file" class="case-document-upload" id="fileupload" name="fileupload"
				title="Browse for document">
			<button type="submit" name="uploaddocument" id="uploaddocument">
				Upload</button>
			</form>
		</li>
		<%
			}

		%>
		<li class="case-document-upload-title">
			<h3>
				Uploaded
			</h3>
		</li>
		<%=Html.DocumentLinks(Model.WorkItem.Documents,Model.WorkItem.WorkItemType) %>
	</ul>
	<%} %>
	<%} // End If Request is Authenticated
   else
   { %>
	<%=Html.ActionLink("Add Document", "AddDocument", Model.WorkItem.WorkItemType.GetDescription() , new {id = Model.WorkItem.Id}, null)%>
	<%} %>
</div>
