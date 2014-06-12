<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>
<% 
	if (RoleAuthorizationService.ProfileViewSignatures())
	{
%>
<hr style="color: #CCCCCC" />
<%
	using (
		Html.BeginForm("Signature", "Profile", FormMethod.Post, new { id = "profilesignature", enctype = "multipart/form-data" })
		)
	{%>
<div class="profile-signature">
	<%=Html.Signature(Model.User, new {@class = "signature", @id = "currentsignature"})%>
	<h1>
		Signatures</h1>
	<div id="signaturelist">
		<h2>
			Uploaded signatures</h2>
		<%=Html.SignatureList(Model.User)%>
		<div class="clear">
		</div>
	</div>
	<div>
		Upload signature:
		<input type="file" id="signatureupload" name="signature" title="signature" />
	</div>
	<div>
		<button type="submit" name="submit" id="savesignature">
			Update</button>
	</div>
	<br />
</div>
<%
	}


	}//End Role Check%>
