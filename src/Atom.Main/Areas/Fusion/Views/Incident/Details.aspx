<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.CaseDetailsViewModel>" %>

<%@ Import Namespace="Atom.Areas.Fusion.Domain.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Atom Fusion --> Incident Details:
	<%=Model.Incident.Id %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
    <link href="<%=Url.Stylesheet("case.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.JavascriptFusion("jquery.MultiFile.js")%>" type="text/javascript"></script>
    <script src="<%=Url.JavascriptFusion("highlight.pack.js") %>" type="text/javascript"></script>
    <script src="<%=Url.JavascriptFusion("workitem/workitem-common.js") %>" type="text/javascript"></script>
    <link href="<%=Url.Stylesheet("vs.css") %>" rel="stylesheet" type="text/css" />
    <%Html.RenderPartial("WMD-Includes"); %>
    <script type="text/javascript">
        $(function () {

            hljs.initHighlightingOnLoad();

            $('#closurereason').change(function () {
                if ($('#closurereason option:selected').text() == 'Please Choose') {
                    alert('Please choose a valid reason!');
                } else {
                    $('#frmclosurereason').submit();
                }

            });

            $('#closecase').click(function () {
                if (checkComment()) {
                    var $frm = $('#frmaddc');
                    var url = $frm.attr('action').replace('AddComment', 'AddCommentAndClose');
                    $frm.attr('action', url).submit();
                }
                return false;
            });

            $('#addcomment').click(function () {
                return checkComment();
            });

			<%if (RoleAuthorizationService.UnitsOfWorkRequired(Model.WorkItem.WorkItemType))
     {%>
		    $('#UnitsOfWork').keypress(function (e) {
		        var keycode = e.which;
		        var keypressed = String.fromCharCode(keycode);
		        var value = $(this).val();

		        return (keycode > 47 && keycode < 58 || keycode == 8 || keycode == 0);
		    });
			<%}%>

		    $('#assignto').change(function () {
		        if ($('#assignto').val() != '') {
		            $('#assign-form').submit();
		        }
		    });
		    $('#assigntodept').change(function () {
		        if ($('#assigntodept').val() != '') {
		            $('#assigndept-form').submit();
		        }
		    });

		    $('#fileupload').MultiFile({
		        max: 1,
		        accept: 'gif,jpg,png,jpeg,doc,docx,pdf,gif,xls,xlsx,ppt,pps,vsd,mpp,msg,xml,txt,csv,zip'
		    });

		    $('#uploaddocument').click(function () {
		        var val = $('#fileupload').val()
		        if (val == '') {
		            alert('Please enter a file to upload');
		            return false;
		        }
		        $('#adddoc').submit();
		        return false;
		    });

		<%if (User.IsInRole("Fusion.ResourceUser") && Model.Incident.IncidentStatus != SupportIncidentStatus.Closed)
    { %>
		    $('#edititem_summary').click(function () {
		        $('#summary')
				.attr('readonly', false)
				.css('border', '1px dashed Red')
				.select();
		        return false;
		    })

		    $('#save_edititem_summary').click(function () {
		        var val = $('#summary').val();
		        if (val == '') {
		            alert('Summary is a reqired Field')
		        }
		        else {
		            if ($('#summary').attr('readonly')) {
		                alert('Summary has not been changed - why save it?');

		            }
		            else {
		                if (confirm('Confirm Summary change?')) {
		                    $('#editsummarysaveform').submit();
		                }

		            }
		        }
		        return false;
		    });
		<%} %>

		});

        function checkComment() {
            var unitsOfWork = $('#UnitsOfWork').val();
            var msg = '';
			<%if (RoleAuthorizationService.UnitsOfWorkRequired(Model.WorkItem.WorkItemType))
     {%>
		    if (unitsOfWork == '') {
		        msg = 'Please ensure units of work is completed.';
		    }
			<%}%>
		    if ($('#wmd-input').val() == '') {
		        msg += "\nPlease ensure comment is completed.";
		    }

		    if (msg != '') {
		        alert(msg);
		        return false;
		    }
		    return true;
		}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="case-details">
        <div class="container_16">
            <div class="grid_2">
                <div class="case-number priority<%=(int) Model.WorkItem.Severity%>">
                    <span>
                        <%=Model.WorkItem.Id%></span>
                </div>
            </div>
            <div class="grid_2">
                <div class="case-status<%=(int)Model.Incident.IncidentStatus %>">
                    <%=Model.Incident.IncidentStatus.GetDescription()%>
                </div>
            </div>
            <div class="grid_12">
                <%using (Html.BeginForm("EditSummary", "Incident", new { id = Model.Incident.Id }, FormMethod.Post, new { id = "editsummarysaveform" }))
                  {%>
                <div style="">
                    <div style="float: left;">
                        <textarea rows="4" cols="4" id="summary" readonly="readonly" style="font-family: Arial,Helvetica,sans-serif; border: solid 1px #CCCCCC; padding: 4px; font-size: 24px; font-weight: bold; height: 60px; width: 660px; background-color: White;"
                            name="summary"><%=Model.Incident.Summary %></textarea>
                    </div>
                    <div style="float: right; padding-right: 5px;">
                        <%if (User.IsInRole("Fusion.ResourceUser") && Model.Incident.IncidentStatus != SupportIncidentStatus.Closed)
                          {  %>
                        <%=Html.EditItemImage("summary") %>
                        <%=Html.EditItemSaveImage("summary")%>
                        <%} %>
                    </div>
                </div>
                <%} %>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="container_16">
            <div class="grid_4">
                <%
                    Html.RenderPartial("WorkItemTasks");
                    Html.RenderPartial("IncidentClosureDetails");
                    Html.RenderPartial("IncidentClosure");
                    Html.RenderPartial("IncidentOverview");
                    Html.RenderPartial("WorkItemLinks");
                    Html.RenderPartial("WorkItemDocuments");
                    Html.RenderPartial("WorkItemSubscriptions");
					
                %>
            </div>
            <div class="grid_12">
                <div id="case-headlinedate">
                    <span class="case-raised">Raised by:
						<%=Model.Incident.CreatedBy.Name%>
                        <%=Model.Incident.CreateDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Incident.CreateDate.FormatDateTimeRelative()%>)</span>
                    <%if (Model.Incident.AlteredDate.HasValue)
                      { %>
                    <span class="case-updated">Last updated:
						<%=Model.Incident.AlteredDate.Value.FormatDateTimeFull()%>&nbsp;(<%=Model.Incident.AlteredDate.Value.FormatDateTimeRelative()%>)</span>
                    <%} %>
                </div>
                <%var i = 0; %>
                <%foreach (var c in Model.Incident.Comments.OrderByDescending(x => x.CreateDate.Value))
                  {

                      i++;%>
                <div class="cmt" id="CommentText_<%=i%>">
                    <%=Html.Avatar(c.CreatedBy, new { @class = "avatar" })%>
                    <div class="commentwrap">
                        <div class="header">
                            Comment by
							<%=c.CreatedBy.Name%>
                            <%=c.CreateDate.Value.FormatDateTimeRelative()%>, (<%=c.UnitsOfWork %>
							Units of Work)
                        </div>
                        <div class="text">
                            <%=c.CommentText%>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <%} %>
                <%if (Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed)
                  { %>
                <h2>Your Comment</h2>
                <%Html.RenderPartial("WorkItemComment");
      }%>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</asp:Content>
