<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%@ Import Namespace="Atom.Main" %>
<%if (RoleManager.IsUserInRole("Fusion.ChangeBoard") || RoleManager.IsUserInRole("Fusion.IT"))
  {%>
<br />
<%using(Html.BeginForm("SubscribeUser", Model.WorkItem.WorkItemType.GetDescription(), new {id=Model.WorkItem.Id},
		FormMethod.Post, new { id = "subscribe-user-form" }))
  {%>
<div class="form-value">
	Subscribe User:<br />
	<br />
	<%=Html.DropDownList("subscribeuser", new SelectList(Model.SubscribeUsers, "userid", "Name", ""), "Please Select", new {style="width:200px;"})%>
	<br />
</div>
<%} %>
<%} %>