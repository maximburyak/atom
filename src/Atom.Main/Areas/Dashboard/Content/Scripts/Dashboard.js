jQuery.fn.center = function () {
	this.css("position", "absolute");
	this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
	this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
	return this;
}

function SwitchPmo() {
	$("tr.PmoDept").toggle();
	$("tr.PmoTeam").toggle();
	saveCookie();
}

function saveCookie() {
	var cookie = $("tr.PmoTeam").is(":visible");
	$.cookie('IT_Dashboard', cookie);
}

function getCookieValue(key) {
	var cookie = $.cookie('IT_Dashboard');
	return cookie;
}

function resetPMO() {
	var showTeam = getCookieValue("ShowPmoTeam");
	if (showTeam=='true') SwitchPmo();
}

function refreshData() {
	$.post('/Dashboard/JsonData', function (data) {
		var json = eval(data);
		var draggable = json.Draggable;
		var droppable = json.Droppable;
		$('td.DashboardItems').html("");
		$('td.SideboardItems').html("");
		refreshCrfData(json.CrfData, draggable, droppable);
		refreshCrfData(json.Unassigned, draggable, droppable);
		refreshCrfData(json.Completed, draggable, droppable);
		refreshCrfData(json.PmoGroup, draggable, droppable);
		refreshStatsData(json.StatsData);
		init();
		textShadowForMSIE();
	});
}

function refreshCrfData(data, draggable, droppable) {
	$.each(data, function () {
		loadCrf(this, draggable);
	});
}

function refreshStatsData(data) {
	$.each(data, function () {
		$('#' + this.Id).html('<br>'+this.Total+'');
	});
}

function loadCrf(data, draggable, droppable) {
	var doctype = (data.Severity == "0") ? "project" : "crf";
	var crf = format('<span class="{0}crf severity{1}" title="{2}" data="{3}">', draggable, data.Severity, data.Title, doctype);
	crf = crf + (format("<p><b class=\"crf-icons\">{0}</b>", data.Type));
	crf = crf + (format("<b class=\"info\" id=\"{0}\" data=\"{1}\" title=\"View Details\"></b>", data.Crf, doctype));
	crf = crf + (format("<br /><b class=\"crfno\" data=\"{0}\">{1}</b>", doctype, data.Crf));
	crf = crf + ("</p></span>");
	$('#' + data.Id).append(crf);
}

function format(str)
{
  for(i = 1; i < arguments.length; i++)
  {
    str = str.replace('{' + (i - 1) + '}', arguments[i]);
  }
  return str;
 }

 function init() {
 	$(".draggable").draggable({ revert: 'invalid' });

 	$(".droppable").droppable({
 		drop: function (event, ui) {
 			$.post("/fusion/" + ui.draggable.attr("data") + "/Assign/" + ui.draggable.find("b").html() + "?assignto=" + $(this).attr("id"), function (data) {
 				$("#loading-msg").center();
 				$("div.loading").show();
 				$("body").load("/dashboard");
 			});
 		}
 	});

 	$(".droppable-PMO").droppable({
 		drop: function (event, ui) {
 			$.post("/fusion/" + ui.draggable.attr("data") + "/AssignToDept/" + ui.draggable.find("b").html() + "?assigntodept=" + $(this).attr("id"), function (data) {
 				$("#loading-msg").center();
 				$("div.loading").show();
 				$("body").load("/dashboard");
 			});
 		}
 	});

 	$("div.case-number").live('click', function (e) {
 		window.open("/Fusion/" + $(this).attr("data") + "/Details/" + $(this).find('span').html());
 	});

 	$(".crfno").click(function (e) {
 		window.open("/Fusion/" + $(this).attr("data") + "/Details/" + $(this).html());
 	});

 	$("b.info").click(function (e) {
 		var $tooltip = $("#html-tooltip");
 		$("#tooltip-content").load("/Fusion/" + $(this).attr("data") + "/Summary/" + $(this).attr("id"), function () {
 			var doch = $tooltip.height();
 			var docw = $tooltip.width();
 			var winh = $(document).height();
 			var winw = $(document).width() - 20;
 			var dimx, dimy
 			dimy = ((winh - e.pageY) < doch) ? (winh - doch - 50) : (e.pageY - 50);
 			dimx = ((winw - e.pageX) < docw) ? (e.pageX - docw - 50) : (e.pageX + 20);
 			$tooltip.css({ "left": dimx + "px", "top": dimy + "px" });
 			$tooltip.show();
 		});
 	});

 	$("div.btnclose").live('click', function (e) {
 		$("#html-tooltip").hide();
 		$("#tooltip-content").html("");
 	});

 	var layoutSettings_Outer = {
 		name: "outerLayout" // NO FUNCTIONAL USE, but could be used by custom code to 'identify' a layout
 		// options.defaults apply to ALL PANES - but overridden by pane-specific settings
	, defaults: {
		size: "auto"
		, minSize: 50
		, paneClass: "pane" 		// default = 'ui-layout-pane'
		, resizerClass: "resizer"	// default = 'ui-layout-resizer'
		, togglerClass: "toggler"	// default = 'ui-layout-toggler'
		, buttonClass: "button"	// default = 'ui-layout-button'
		, contentSelector: ".content"	// inner div to auto-size so only it scrolls, not the entire pane!
		, contentIgnoreSelector: "span"		// 'paneSelector' for content to 'ignore' when measuring room for content
		, togglerLength_open: 135			// WIDTH of toggler on north/south edges - HEIGHT on east/west edges
		, togglerLength_closed: 135			// "100%" OR -1 = full height
		, hideTogglerOnSlide: true		// hide the toggler when pane is 'slid open'
		, togglerTip_open: "Close"
		, togglerTip_closed: "Open"
		, resizerTip: "Resize"
		//	effect defaults - overridden on some panes
		, fxName: "slide"		// none, slide, drop, scale
		, fxSpeed_open: 500
		, fxSpeed_close: 500
		, fxSettings_open: { easing: "" }
		, fxSettings_close: { easing: "" }
	}
	, north: {
		size: 110
		, spacing_closed: 70			// wider space when closed
		, spacing_open: 21			// wider space when closed
		, togglerLength_closed: 135			// make toggler 'square' - 21x21
		, togglerAlign_closed: "center"		// align to top of resizer
		, togglerLength_open: 135			// NONE - using custom togglers INSIDE west-pane
		, togglerTip_open: ""
		, togglerTip_closed: ""
		, resizerTip_open: ""
		//, slideTrigger_open: "click" 	// default
		, initClosed: true
		, resizable: false
		, slidable: false
		, north__togglerLength_closed: -1
	}
	, west: {
		size: 250
		, spacing_closed: 21			// wider space when closed
		, spacing_open: 21			// wider space when closed
		, togglerLength_closed: 135			// make toggler 'square' - 21x21
		, togglerAlign_closed: "middle"		// align to top of resizer
		, togglerLength_open: 135			// NONE - using custom togglers INSIDE west-pane
		, togglerTip_open: "Close"
		, togglerTip_closed: "Open"
		, resizerTip_open: "Resize"
		, slideTrigger_open: "click" 	// default
		, initClosed: true
		, resizable: false
		, slidable: false
	}
	, east: {
		size: 250
		, spacing_closed: 21			// wider space when closed
		, spacing_open: 21			// wider space when closed
		, togglerLength_closed: 135			// make toggler 'square' - 21x21
		, togglerAlign_closed: "middle"		// align to top of resizer
		, togglerLength_open: 135			// NONE - using custom togglers INSIDE west-pane
		, togglerTip_open: "Close"
		, togglerTip_closed: "Open"
		, resizerTip_open: "Resize"
		, slideTrigger_open: "click"
		, initClosed: true
		, resizable: false
		, slidable: false
	}
	, center: {
		paneSelector: "#mainContent" 			// sample: use an ID to select pane instead of a class
		, onresize: "innerLayout.resizeAll"	// resize INNER LAYOUT when center pane resizes
		, minWidth: 200
		, minHeight: 200
	}
 	};
 	$("body").layout(layoutSettings_Outer);
 	resetPMO();
 }

 $(function () {
 	var refreshId = setInterval(function () {
 		refreshData();
 	}, 180000);

 	init();
 	refreshData();
 });