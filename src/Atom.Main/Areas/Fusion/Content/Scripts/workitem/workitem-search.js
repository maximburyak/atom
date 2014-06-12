var aSearch = "";
$(function() {
	// Do we have a default filter?
	InitSearch = ($("#filter-list option:selected[text$='[DEFAULT]']").length > 0 && (InitSearch == $("#filter-list").val() || InitSearch == "")) ? $("#filter-list").val() : InitSearch;
	//Populate search box with query
	$("#searchbox").val(InitSearch);

	//Add infinite scroll hander
	$(window).scroll(function() {
		if (onscrollVar && ($(window).scrollTop() == $(document).height() - $(window).height())) {
			onscrollVar = false;
			listcases(InitSearch, true, null);
		}
		return false;
	});

	$("#filter-list").change(function () {
		if ($(this).val() != "") {
			$("#filters-change").addClass("filter-loading");
			setTimeout("changeFilter()", 1000);
		}
		return false;
	});

	$("#filter-save").click(function() {
		if ($("#filterlabel").val() != '') {
			$("#filter-outcome").addClass("filter-loading");
			$(this).attr("disabled", true);
			$("#filter-outcome").show();

			$.postJSON("/Fusion/Search/FilterSave", {
				searchFilter: $("#searchFilter").val(),
				searchMnemonic: $("#filterlabel").val()
			}, function(json) {
				setTimeout("setFilterSuccess(" + eval(json) + ")", 3000);
			},
						function(json, textStatus) {
							$(this).attr("disabled", false);
							$("#filter-outcome").removeClass("filter-loading").addClass("filter-error");
							alert("Serious error: " + textStatus);
						}
					);
		}
		return false;
	});
	if (InitSearch == "")
		InitSearch = null;

	//Fire the ajax on load
	listcases(InitSearch, true, null);

});                            // End jQ onready

function listcases(search, append, maxcase) {
	var searchToPerform = (search == null) ? modelSearch : search;
	var maxCase = (maxcase == null) ? ($("#caselist [class^='workitempriority']:last").attr("id") || 0) : maxcase;
	var actionToPerform = (search == null) ? modelAction : "Query";
	$("#searchbox").val(searchToPerform);
	$.postJSON("/Search/" + actionToPerform + "/", {
		search: searchToPerform,
		maxcase: maxCase
	}, function(json) {
		listcasesCallback(json, append);
	},
			function(json, textStatus) {
				//alert('error: ' + textStatus);
			});
}
function isDigit(s) {
	digit = new RegExp(/^[0-9]+(\.[0-9]{1,2})?$/)
	return digit.test(s);
}

function listcasesCallback(list, append) {
	var searchBoxValue = $("#searchbox").val();
	if (list.Items.length == 1 && isDigit(searchBoxValue)) {
		var c = list.Items[0];
		document.location.href = "/Fusion/" + c.Controller + "/Details/" + c.Id;
	}
	else {
		setFilterHandler(list);

		$("#filters-change").removeClass("filter-loading");
		if (list.Items.length > 0) {
			$("#end-results").hide();
			var html = parseTemplate($("#casetemplate").html(), { cases: list.Items });
			if (append) {
				$(html).appendTo("#caselist");
			}
			else {
				$("#caselist").html(html);
			}
			onscrollVar = true;
			setupHandlers();
		}
		else {
			if (!append) { $("#caselist").html(""); }

			$("#end-results").show();
			onscrollVar = false;
		}
		$("#filters-displaycount").html("Results: " + $("#caselist [class^='caselistitem']").length).show();
		return false;
	}
}

function setFilterSuccess(success) {
	if (success) {
		$("#filter-outcome").removeClass("filter-loading").addClass("filter-success");
		$("#filters-description-content").slideUp("slow");
		filters();
	}
	else {
		$("#filter-save").attr("disabled", false);
		$("#filter-outcome").removeClass("filter-loading").addClass("filter-error");
		alert("An error occurred during save");
	}
	return false;
}

function setupHandlers() {
	$("#caselist [class^='workitempriority']").click(function() {
		document.location.href = $(this).attr("href");
	});
	$("#caselist [class^='workitempriority']").mouseover(function() {
		var id = $(this).attr("rel");
		$(this).removeClass("workitempriority" + id);
		$(this).addClass("workitemprioritymo" + id);
	});
	$("#caselist [class^='workitempriority']").mouseout(function() {
		var id = $(this).attr("rel");
		$(this).removeClass("workitemprioritymo" + id);
		$(this).addClass("workitempriority" + id);
	});
	return false;
}

function setFilterHandler(list) {

    if (list.SearchDisplayText != "" && list.SearchFilter != "") {
		$("#searchFilter").val(list.SearchFilter);
		$("#filters-displaytext").html("You searched for items: " + list.SearchDisplayText);
        $("#filters-savecurrent").show().click(function() {
			$("#filterlabel").val("");
			$("#filter-save").attr("disabled", false);
			if ($("#filters-description-content").css("display") == "none") {
				$("#filters-description-content").slideDown();
			}
			else {
				$("#filters-description-content").slideUp();
			}
			$("#filter-outcome").removeClass("filter-loading filter-error filter-success");
			$("#filterlabel").focus();
			return false;
		});
	}
	return false;
}

function changeFilter() {
    InitSearch = $("#filter-list").val();
    filterChanged();
    if (InitSearch != "") {
        listcases(InitSearch, false, 0);
    }
}

function filterChanged() {
    $.postJSON("/Search/FilterChanged/", {
        filter: InitSearch
    });
	return false;
}

function filters() {
	// re-load filters.
	$.postJSON('/Fusion/Search/Filters', null, function(data) {
		$("#filter-list").fillSelect(data, true).focus();
	},
			function(json, textStatus) {
				alert("error has occurred: " + textStatus);
			});
	return false;
}