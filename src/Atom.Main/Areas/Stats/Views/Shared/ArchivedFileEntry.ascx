<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Atom.Main.Areas.Stats.Services" %>
<tr>
	<td>
		<img src="../../../../Content/images/arrow_undo.png" class="restoreButton" title="Restore this file to the webserver" alt="Restore" />
	</td>
	<% Html.RenderPartial("RowColumns", Model ); %>
	
</tr>
