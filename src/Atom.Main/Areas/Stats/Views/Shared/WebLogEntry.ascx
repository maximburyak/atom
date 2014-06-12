<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Atom.Main.Areas.Stats.Services" %>
<%@ Import Namespace="System.ComponentModel" %>
<tr>
	<td class="actionButtons" >
			<img src="../../../../Content/images/folder_page.png" class="archiveButton" title="Archive this file" alt="Archive" />
	</td>
	
	<% Html.RenderPartial("RowColumns", Model ); %>
</tr>
