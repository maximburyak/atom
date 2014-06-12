<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>
<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
<% using (Html.BeginForm("Index", "Profile", FormMethod.Post, new { id = "profile", enctype = "multipart/form-data" }))
   {%>
<div class="profile">
	<%=Html.Avatar(Model.User, new {@class = "avatar", @id="currentavatar"})%>
	<h1>
		<%=Model.User.Name%>
		(<%=Model.User.UserID%>)</h1>
	<div id="avatarlist">
		<h2>
			Uploaded avatars</h2>
		<%=Html.AvatarList(Model.User)%>
		<div class="clear">
		</div>
	</div>
	<div>
		Upload avatar:
		<input type="file" id="avatarupload" name="avatar" title="avatar" />
	</div>
	<div>
		<button type="submit" name="submit" id="saveavatar">
			Update</button>
	</div>
	<br />
</div>
<% }%>
