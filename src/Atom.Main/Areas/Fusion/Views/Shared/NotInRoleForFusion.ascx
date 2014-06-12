<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<div id="fixed" style="width: 600px;">
	<div id="pagetitle">
		<h1>
			Not In Role</h1>
	</div>
	<div>
	</div>
	<div class="box-error">
		<h3>
			Message: Insufficient Role Privileges
		</h3>
		<br />
		<p class="form-summary">
			<%=Model.ErrorMessage%>
		</p>
	</div>
</div>
