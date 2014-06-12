<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<Atom.Main.Areas.Fusion.Models.ViewModels.AddCaseViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Add New Incident
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
	<%if (RoleAuthorizationService.IncidentCreateView())
   {%>
	<script src="<%=Url.Javascript("jquery.validate.js") %>" type="text/javascript"></script>
	<script src="<%= Url.JavascriptFusion("jquery.MultiFile.js")%>" type="text/javascript"></script>
	<script src="<%=Url.JavascriptFusion("xVal.jquery.validate.js") %>" type="text/javascript"></script>
	<%Html.RenderPartial("WMD-Includes"); %>
	<script type="text/javascript">
		  
	var incidentAdditionalInfo = eval('<%=Model.IncidentAdditionalInfo%>');
	var predeterminedFinished=false;
	$(function() {
	    $('#ApplicationRow').hide();
	    $("#additionalinformationcontainer").hide();
		if(incidentAdditionalInfo) {
			if(incidentAdditionalInfo.length>0) {
				// Create Pre-determined additional information.
				$('#predetermined-additionalinfo').createPredeterminedAdditionalInfo(incidentAdditionalInfo,'');
				predeterminedFinished=true;
			}
			predeterminedFinished=true;
		}
		else {
			predeterminedFinished=true;
		}
		
		<%=Html.ClientSideValidation("",RuleSet.Empty).SuppressScriptTags()
		.AddRule("System_Area_Id", new RequiredRule())
		.AddRule("Summary", new RequiredRule())
		.AddRule("Location", new RequiredRule())
		.AddRule("Severity", new RequiredRule())
		.AddRule("Hub.Id", new RequiredRule())
		.AddRule("wmd-input", new RequiredRule()) %>
		
			//Category choice
			$('#System_Area_Id').change(function() {
				var id = $(this).val();
				var areacategories = $('#System_Category_Id');
				areacategories.attr('disabled', id == "");
                
			    if (id == '') {
					areacategories.clearSelect(true);
					$("#additionalinfocontainer").hide();
					$('#additionalinformation').html('');
					$('#ApplicationRow').hide();
				} else {
			        if (id == '56') {
			            $('#ApplicationRow').show();
			            $.postJSON('AreaCategories', { SupportArea: id }, function (data) {
			                    areacategories.fillSelect(data, true);
			                },
			                function (data, textStatus) {
			                    alert('error: ' + textStatus);
			                }
			            );
			        } else {
			            $('#ApplicationRow').hide();
			        }
			    }
			});
			
			$('#fileupload').MultiFile({
				max: 1,
				accept: 'gif,jpg,png,jpeg,doc,docx,pdf,xls,xlsx,ppt,pps,vsd,mpp,msg'
			});
						
			//Additional Information choice
			 $('#System_Category_Id').change(function() {
				var id = $(this).val();
				
				if(id !='') {
				    $("#additionalinformationcontainer").show(function () {
						$.postJSON('CategoryAdditionalInfo', { categoryId: id }, function(data) {
							$('#additionalinformation').createAdditionalInfo(data,incidentAdditionalInfo);
						},
							function(data, textStatus) {
								alert('error: ' + textStatus);
							}
						);	
					});
				}
				else {
				    $("#additionalinformationcontainer").hide();
					$('#additionalinformation').html('');
				}
			});
			
			$("#createcase").click(function(e) {
				window.onbeforeunload = null; 
				$(this).attr("disabled",true);
				$("#addform").submit();
				e.stopPropagation();
			});
			
			$("#addform").bind('change keydown', function() {
				window.onbeforeunload = function() { return "You have chosen to cancel the creation of an Incident. Any completed information will be lost. Are you sure?"; }
				$("#createcase").attr("disabled",false);
			});
		});
		
		function AdditionalInfoPopulator(infotypeid) {
		if(incidentAdditionalInfo) {
		
			for (var i=0; i < incidentAdditionalInfo.length;i++) {
				if (incidentAdditionalInfo[i].Id == infotypeid) {
					return incidentAdditionalInfo[i].Value;
				}
			}
			return "";
			}
			return "";
		}
		
		function CreateAdditionalInformationNeeded(infotypeid)
		{
			if(!incidentAdditionalInfo) {
				return true;
			}
			for (var i=0; i < incidentAdditionalInfo.length;i++) {
				if (incidentAdditionalInfo[i].Id == infotypeid && predeterminedFinished) {
					return false;
				}
			}
			return true;
		}
				
	</script>
	<script id="itemtemplate" type="text/html">
	 <# for(var i=0; i < items.length; i++)     
		{       
			var item = items[i]; 
			if (CreateAdditionalInformationNeeded(item.Id)){#>
			<p><label for="<#= item.Id #>"><#= item.Description #></label>
			<input type="text" name="AdditionalInfo[<#= i #>].Value" <#if(!predeterminedFinished) {#> readonly="readonly" <#}#> value="<#=AdditionalInfoPopulator(item.Id) #>" />
			<input type="hidden" name="AdditionalInfo[<#= i #>].InfoType.Id" value="<#= item.Id #>" />
			<input type="hidden" name="AdditionalInfo[<#= i #>].InfoType.Description" value="<#= item.Description #>" />
			</p>
	 <# }
	 } #>
	</script>
	<script type="text/javascript">

	</script>
	<%
		}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container_12">
		<div class="grid_10 prefix_1">
			<div class="form" id="case-add">
				<%Html.RenderPartial("IncidentAdd"); %>
			</div>
		</div>
	</div>
</asp:Content>
