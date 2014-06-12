<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Stats/Views/Shared/Stats.Master" Inherits="System.Web.Mvc.ViewPage<ListArchivedWebFilesViewModel>" %>
<%@ Import Namespace="BeValued.Utilities.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Show Archived Files
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleBodyContent" runat="server">
	Show Archived Files
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<script type="text/javascript">
		
		var sortColumn = "<%=PropertyUtil.GetName<ArchivedFile>(x => x.CreatedDate) %>";
		var isSortDesc = true;
		
		function restoreFileHandler (event)  {// grab all the row data
			var $button = $(event.target);
			var $row = $button.parentsUntil("TBODY").last();
			// get all the data/fields (TDs) for the current row
			var $td = $row.find("TD");
			$.post(	"/Stats/Weblog/Restore",
					{
						Id:				colData($td, "Id"),
						FileId:			colData($td, "FileId"),
						FileName:		colData($td, "FileName"),
						Website:		colData($td, "Website"),
						Webpath:		colData($td, "Webpath"),
						NativePath:		colData($td, "NativePath")
					},
					function () { // success handler
						$row.remove();
						reZebraStripe(".weblogResults tbody");
					}
			);
		}

		function colData($td, colName) {
			var rawString = $td.filter("[columnName=" + colName + "]").text();
				
			// trim the string
			return rawString.replace(/^\s+|\s+$/g, "");
		}

		function sendQuery() {
			
			var $resultsBox = $("#ArchivedWebFilesTable");
			displayLoadingMessage($resultsBox);
			$resultsBox.load(
				"/Stats/WebLog/ListArchived",
				{
					SortColumn: sortColumn,
					IsSortDesc: isSortDesc,
					Website:	$("#Websites").val(),
					Webpath:	$("#PathFilter").val()
				},
				// success handler
				function (responseText, textStatus, smlHttpRequest) {
					$("#hiddenColumnsBorder").hide();
					decorateTable();
				}
			);
		}

		function decorateTable() {

			$(".restoreButton").click(restoreFileHandler);

			linkToWebPathFilter("tbody [columnname=Webpath]");

			// sort column event handler
			$(".ClickSortable").click(sortHandler);
			autoHideCols(".weblogResults", "#hiddenColumns");
			zebraStripe(".weblogResults");
		}
	</script>


	<div align="center"  id="ArchivedWebFilesTable">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadInclude" runat="server">


</asp:Content>
