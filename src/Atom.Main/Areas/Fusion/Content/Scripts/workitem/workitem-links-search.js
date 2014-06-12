$(function () {

    $(function () {
        //Populate search box with query
        $("#listsearchbox").val(InitSearch);
        //Fire the ajax on load
        listcases(InitSearch, true, null, modelSearch, modelAction, currentWorkItemId);
    });   // End jQ onready

    $("#saveLinks").click(function () {
        $("#linkworkitems-form").submit();
        return false;
    });

});

function listcases(search, append, maxcase) {
    var searchToPerform = (search == null) ? modelSearch : search;
    var maxCase = (maxcase == null) ? ($("#caselist [class^='workitempriority']:last").attr("id") || 0) : maxcase;
    var actionToPerform = (search == null) ? modelAction : "LinkableWorkItems";
    $.postJSON("/Search/" + actionToPerform + "/", {
        search: searchToPerform,
        maxcase: maxCase,
        currentWorkItemId: currentWorkItemId
    }, function (json) {
        listcasesCallback(json, append);
    },
			function (json, textStatus) {
			    alert('error: ' + textStatus);
			});
}

function listcasesCallback(list, append) {
    var searchBoxValue = $("#linksearchbox").val();
    if (list.Items.length > 0) {
        $("#end-results").hide();
        $("#save-links").hide();
        var html = parseTemplate($("#casetemplate").html(), { cases: list.Items });
        if (append) {
            $(html).appendTo("#caselist");
        }
        else {
            $("#caselist").html(html);
        }
        $("#save-links").show();
    }
    else {
        if (!append) { $("#caselist").html(""); }
        $("#save-links").hide();
        $("#end-results").show();
    }
    return false;
}

