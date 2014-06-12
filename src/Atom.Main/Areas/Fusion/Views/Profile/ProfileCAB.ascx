<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.ProfileViewModel>" %>

<% 
    if (RoleAuthorizationService.ProfileViewAutoAssignTo())
    {
%>
<hr style="color: #CCCCCC" />
<div class="changeboardmeeting">
    <div>
        <span>
            <img alt="Next Change Board Meeting" id="changeboardmeeting" src="/Areas/Fusion/Content/Images/icons/cab.png" />
            <h1>
                Next Change Board Meeting</h1>
        </span>
    </div>
    <div>
        <% using (Html.BeginForm("UpdateChangeBoardMeetingDate", "Profile", FormMethod.Post, new { id = "profilechangeboardmeeting" }))
           {%>
        <p>
            <label for="DateOfNextChangeBoardMeeting">
                Date of next change board meeting<span class="required-field"></span></label>
            <%= Html.TextBox("ChangeBoardMeetingDate", Model.ChangeBoardMeetingDate.ToShortDateString()) %>
            <%= Html.ValidationMessage("ChangeBoardMeetingDate", "*") %>
        </p>
        <button id="updateChangeBoardMeeting" type="submit">
            Update</button>
        <%} %>
    </div>
</div>
<%} %>
