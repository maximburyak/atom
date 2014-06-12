<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.AddCrfViewModel>" %>
<%@ Import Namespace="Atom.Main" %>

<%if (!RoleAuthorizationService.CrfCreateView())
  {
      Model.ErrorMessage = "Unfortunately you do not currently have access to \"Create CRF\" function, please discuss any comments or concerns you may have with your line manager.";
%>
<%
    Html.RenderPartial("NotInRoleForFusion", Model); %>
<%}
  else
  { %>
<div class="box-error" id="messagebox" style="<%=Html.ErrorBoxDisplay(ViewData.ModelState.IsValid)%>">
    <h3>
        The following errors were encountered:</h3>
    <ul>
    </ul>
    <%=Html.ValidationSummary()%>
</div>
<br />
<% using (Html.BeginForm("Add", "Crf", FormMethod.Post, new { enctype = "multipart/form-data", id = "addform" }))
   {%>
<%=Html.Hidden("Id","0") %>
<fieldset class="fs-crf">
    <legend>Create Change Request</legend>
    <div>
        <label for="Summary">
            Change Title <span class="required-field"></span>
        </label>
        <%=Html.TextBox("Summary", "", new {maxLength=255}) %>
        <%= Html.ValidationMessage("Summary", "*")%>
    </div>
    <br />
    <div>
        <label for="RequestedCompletionDate">
            Completion Date
        </label>
        <%=Html.TextBox("RequestedCompletionDate", "")%>
        <%= Html.ValidationMessage("RequestedCompletionDate", "*")%>
    </div>
    <br />
    <div>
        <div>
            <label for="emergencychange">
                Emergency Change?<span class="required-field"></span>
            </label>
        </div>
        <div style="margin-left: 150px">
            <span style="width: 50px;">Yes</span>
            <%=Html.RadioButton("emergencychange",1,Model.WorkItemIsEmergencyChange(),new {id="emergency",style="margin-left: 5px;width: 50px"}) %>
            <span style="width: 50px;">No</span><%=Html.RadioButton("emergencychange", 0, !Model.WorkItemIsEmergencyChange(), new { id = "noemergency", style = "margin-left: 5px;width: 50px" })%>
        </div>
    </div>
    <br />
    <div>
        <label for="ClientRequirement">
            Client Requirement?
        </label>
        <%=Html.CheckBox("ClientRequirement", false,new {style="width:30px;"}) %>
    </div>
    <br />
    <%if (RoleManager.IsUserInRole("Fusion.IT"))
      {%>
    <div>
        <label for="IsHouseKeeping">
            Is House Keeping?</label>
        <%= Html.CheckBox("IsHouseKeeping", false, new { style = "width:30px;" })%>
    </div>
    <br />
    <%}
      else
      {%>
    <%=Html.Hidden("IsHouseKeeping",false) %>
    <%} %>
    <div>
        <label for="CreateDate">
            Request Date
        </label>
        <%=Html.TextBox("CreateDateDisplay", DateTime.Now, new { @class = "input-readonly" })%>
        <%= Html.ValidationMessage("CreateDateDisplay", "*")%>
    </div>
    <br />
    <div>
        <label for="CreatedBy">
            Requested By
        </label>
        <%=Html.TextBox("CreatedBy",Model.User.Name, new {@class="input-readonly"})%>
        <%= Html.ValidationMessage("CreatedBy", "*")%>
    </div>
    <br />
    <div>
        <label>
            Description of Change<br />
            (min 50 characters) <span class="required-field"></span>
        </label>
        <div style="margin-left: 150px; width: 700px; background-color: White; padding: 5px;
            border: 1px solid #ccc;">
            <div style="">
                <%=Html.Hidden("Comments[0].UnitsOfWork","0") %>
                <div id="wmd-button-bar" class="wmd-panel">
                </div>
                <textarea style="position: relative; display: block;" id="wmd-input" class="wmd-panel"
                    name="Comments[0].CommentText"><%if (Model.WorkItem.Comments != null)
                                                     { %><%= Model.WorkItem.Comments[0].CommentText %><%} %></textarea>
                <div id="wmd-preview" class="wmd-panel">
                </div>
                <br />
            </div>
        </div>
    </div>
    <br />
    <div>
        <h2>
            Scope of Change</h2>
    </div>
    <br />
    <div>
        <div class="filter-list">
            <label for="ChannelsOut">
                Channels<span class="required-field"></span></label>
            <div class="filter-list">
                <span>Available</span>
                <%= Html.DropDownList("ChannelsOut", new SelectList(Model.Channels, "Id", "Description"), new { multiple = "multiple" })%>
            </div>
        </div>
        <div class="filter-list" id="filter-list-buttons-channel">
            <button disabled="disabled" id="channelAdd">
                &gt;&gt;</button>
            <button disabled="disabled" id="channelRemove">
                &lt;&lt;</button>
        </div>
        <div class="filter-list">
            <div class="filter-list">
                <span>Selected</span>
                <% if (Model.SelectedChannels != null)
                   {%>
                <%=Html.DropDownList("Channels", new SelectList(Model.SelectedChannels, "Id", "Description"),
                                        new { multiple = "multiple" })%>
                <% }
                   else
                   { %>
                <%=Html.DropDownList("Channels", new SelectList(new List<string>(), "Id", "Description"),
                                        new {multiple = "multiple"})%>
                <% } %>
            </div>
        </div>
        <div class="clear" />
    </div>
    <!--Inscos-->
    <div>
        <div class="filter-list">
            <label for="InsuranceCompaniesOut">
                Insurance Companies<span class="required-field"></span></label>
            <div class="filter-list">
                <span>Available</span>
                <%= Html.DropDownList("InsuranceCompaniesOut", new SelectList(Model.InsuranceCompanies, "Id", "Name"), new { multiple = "multiple" })%>
            </div>
        </div>
        <div class="filter-list" id="filter-list-buttons-inscos">
            <button disabled="disabled" id="insuranceCompanyAdd">
                &gt;&gt;</button>
            <button disabled="disabled" id="insuranceCompanyRemove">
                &lt;&lt;</button>
        </div>
        <div class="filter-list">
            <div class="filter-list">
                <span>Selected</span>
                <% if (Model.SelectedInsuranceCompanies != null)
                   {%>
                <%=Html.DropDownList("InsuranceCompanies", new SelectList(Model.SelectedInsuranceCompanies, "Id", "Name"),
                                        new { multiple = "multiple" })%>
                <% }
                   else
                   { %>
                <%=Html.DropDownList("InsuranceCompanies", new SelectList(new List<string>(), "Id", "Name"),
                                        new {multiple = "multiple"})%>
                <% } %>
            </div>
        </div>
        <div class="clear" />
    </div>
    <!-- Suppliers-->
    <div>
        <div class="filter-list">
            <label for="SuppliersOut">
                Suppliers<span class="required-field"></span></label>
            <div class="filter-list">
                <span>Available</span>
                <%= Html.DropDownList("SuppliersOut", new SelectList(Model.Suppliers, "Id", "Name"), new { multiple = "multiple" })%>
            </div>
        </div>
        <div class="filter-list" id="filter-list-buttons-suppliers">
            <button disabled="disabled" id="supplierAdd">
                &gt;&gt;</button>
            <button disabled="disabled" id="supplierRemove">
                &lt;&lt;</button>
        </div>
        <div class="filter-list">
            <div class="filter-list">
                <span>Selected</span>
                <% if (Model.SelectedChannels != null)
                   {%>
                <%=Html.DropDownList("Suppliers", new SelectList(Model.SelectedSuppliers, "Id", "Name"),
                                        new { multiple = "multiple" })%>
                <% }
                   else
                   { %>
                <%=Html.DropDownList("Suppliers", new SelectList(new List<string>(), "Id", "Name"),
                                        new {multiple = "multiple"})%>
                <% } %>
            </div>
        </div>
        <div class="clear" />
    </div>
    <!-- Product Groups-->
    <div>
        <div class="filter-list">
            <label for="ProductGroupsOut">
                Product Groups<span class="required-field"></span></label>
            <div class="filter-list">
                <span>Available</span>
                <%= Html.DropDownList("ProductGroupsOut", new SelectList(Model.ProductGroups, "Id", "Name"), new { multiple = "multiple" })%>
            </div>
        </div>
        <div class="filter-list" id="filter-buttons-list-productgroups">
            <button disabled="disabled" id="productgroupsAdd">
                &gt;&gt;</button>
            <button disabled="disabled" id="productgroupsRemove">
                &lt;&lt;</button>
        </div>
        <div class="filter-list">
            <div class="filter-list">
                <span>Selected</span>
                <% if (Model.SelectedProductGroups != null)
                   {%>
                <%=Html.DropDownList("ProductGroups", new SelectList(Model.SelectedProductGroups, "Id", "Name"),
                                        new { multiple = "multiple" })%>
                <% }
                   else
                   { %>
                <%=Html.DropDownList("ProductGroups", new SelectList(new List<string>(), "Id", "Name"),
                                        new {multiple = "multiple"})%>
                <% } %>
            </div>
            <div class="clear" />
        </div>
        <div>
            <label for="OtherScope">
                Other Scope</label>
            <textarea id="OtherScope" name="OtherScope" rows="5" cols="5"><%if (Model.Crf.OtherScope != null)
                                                                            { %><%=Model.Crf.OtherScope %><%} %></textarea>
            <%= Html.ValidationMessage("OtherScope", "*")%>
        </div>
        <div>
            <label for="BusinessBenefit">
                Business Benefit
                <br />
                (min 50 characters)<span class="required-field"></span></label>
            <textarea id="BusinessBenefit" name="BusinessBenefit" rows="5" cols="5"><%if (Model.Crf.BusinessBenefit != null)
                                                                                      { %><%=Model.Crf.BusinessBenefit %><%} %></textarea>
            <%= Html.ValidationMessage("BusinessBenefit", "*")%>
        </div>
        <div>
            <label for="Alternatives">
                Alternatives<span class="required-field"></span></label>
            <textarea id="Alternatives" name="Alternatives" rows="5" cols="5"><%if (Model.Crf.Alternatives != null)
                                                                                { %><%=Model.Crf.Alternatives %><%} %></textarea>
            <%= Html.ValidationMessage("Alternatives", "*")%>
        </div>
        <div>
            <label for="Severity">
                Severity Status<span class="required-field"></span></label>
            <%= Html.DropDownList("Severity", new SelectList(Model.Severity(), "Key", "Value"), "Please choose Severity status")%>
            <%= Html.ValidationMessage("Severity", "*")%>
        </div>
        <br />
        <div>
            <label for="fileupload">
                Upload Attachment</label>
            <input type="file" class="case-document-upload" id="fileupload" name="fileupload"
                title="Browse for document" /></div>
        <button id="createcrf" type="submit">
            Create</button>
</fieldset>
<% } %>
<%} %>
