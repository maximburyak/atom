<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Stats/Views/Shared/Stats.Master" Inherits="System.Web.Mvc.ViewPage<ListWebLogViewModel>" %>
<%@ Import Namespace="BeValued.Utilities.Utilities" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Find Trends in the Web Logs
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TitleBodyContent" runat="server">
	Find Trends in the Web Logs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="WebLogSearchFilters">

		<script type="text/javascript">
			var sortColumn = "<%=PropertyUtil.GetName<WebLogAnalysis>(x => x.LastAccessed) %>";
			var isSortDesc = true;
			var resultsBatchSize = <%=Model.ResultsBatchSize %>;
			var maxResults = resultsBatchSize;

			$(document).ready(function() {
				var $from = $("#FromDate");
				var $to   = $("#ToDate");

				var defaultOptions = {
					dateFormat: "dd/mm/yy"
				};

				$from.datepicker(defaultOptions);
				//$from.datepicker( "option", "dateFormat", "dd/mm/yy");
				$to.datepicker(defaultOptions);
			});

			function sendQuery() {
				var $resultsBox = $("#WebLogAnalysisPlaceHolder");
				displayLoadingMessage($resultsBox);
				$resultsBox.load(
					"/Stats/WebLog",
					{
						FromDate:			$("#FromDate").val(),
						ToDate:				$("#ToDate").val(),
						PathFilter:			$("#PathFilter").val(),
						SortColumn:			sortColumn,
						IsSortDesc:			isSortDesc,
						MaxResults:			maxResults,
						Website:			$("#Websites").val(),
						AccessCountFrom:	$("#AccessCountFrom").val(),
						AccessCountTo:		$("#AccessCountTo").val()
					},
					// Success handler
					function (responseText, textStatus, xmlHttpRequest) {
						$("#hiddenColumnsBorder").hide();
						// more results button event handler
						$("#MoreResults").click(moreResultsHandler);

						// sort column event handler
						$(".ClickSortable").click(sortHandler);

						// archive web file event handler
						$(".archiveButton").click(archiveHandler);

						linkToWebPathFilter("tbody [columnname=WebPath]");

						zebraStripe(".weblogResults tbody");
						autoHideCols(".weblogResults", "#hiddenColumns");

					}
				);
			}

			function archiveHandler(event) {
				var $button = $(event.target);

				// get all the data/fields (TDs) for the current row
				var $row = $button.parentsUntil("TBODY").last();
				var $td = $row.find("TD");

				$.post(	"WebLog/Archive", 
						{ 
							Id:			null,
							FileId:		colData($td, "Id"), 
							FileName:	colData($td, "FileName"),
							Website:	colData($td, "Website"), 
							Webpath:	colData($td, "WebPath"),
							NativePath:	colData($td, "Path")
						},
						function () { // success handler
							$row.remove();
							reZebraStripe(".weblogResults tbody");
						});
				
			}

			function colData($td, colName) {
				var rawString = $td.filter("[columnName=" + colName + "]").text();
				
				// trim the string
				return rawString.replace(/^\s+|\s+$/g, "");
			}



			function moreResultsHandler(event) {
				
				maxResults += resultsBatchSize;
				sendQuery();

				return false;
			}
		</script>
	
		
	</div>
	<div align="center" id="WebLogAnalysisPlaceHolder" >
	</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ExtendedSearchOptions" runat="server"> 
							
							<table>
							<tr>
								<td>Accessed From:</td>	<td class="dateInput"><%=Html.TextBox("FromDate") %> </td>
								<td>To:			  </td> <td class="dateInput"><%=Html.TextBox("ToDate")%>	   </td>	
							</tr>
							<tr>
								<td>Access Count From:</td>	<td class="numberInput"><%=Html.TextBox("AccessCountFrom")%> </td>
								<td>			   To:</td> <td class="numberInput"><%=Html.TextBox("AccessCountTo")%>	 </td>
							</tr>
							</table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">
</asp:Content>
