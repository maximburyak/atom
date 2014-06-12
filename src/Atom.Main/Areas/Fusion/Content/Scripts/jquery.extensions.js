//New function below allows an on-Error call. where as using jquery $.post does not.
$.postJSON = function(url, params, success, error) {
	var JsonCallParams = {};
	JsonCallParams.type = "POST";
	JsonCallParams.url = url;
	JsonCallParams.processData = true;
	JsonCallParams.data = params;
	JsonCallParams.dataType = "json";
	JsonCallParams.success = success;
	if (error) {
		JsonCallParams.error = error;
	}

	$.ajax(JsonCallParams);
}

$.getJSON = function(url, params, success, error) {
    var JsonCallParams = {};
    JsonCallParams.type = "GET";
    JsonCallParams.url = url;
    JsonCallParams.processData = true;
    JsonCallParams.data = params;
    JsonCallParams.dataType = "json";
    JsonCallParams.success = success;
    if (error) {
        JsonCallParams.error = error;
    }

    $.ajax(JsonCallParams);
}

$.fn.setCursorPosition = function(pos) {
	if ($(this).get(0).setSelectionRange) {
		$(this).get(0).setSelectionRange(pos, pos);
	} else if ($(this).get(0).createTextRange) {
		var range = $(this).get(0).createTextRange();
		range.collapse(true);
		range.moveEnd('character', pos);
		range.moveStart('character', pos);
		range.select();
	}
}

$.fn.autoscroll = function() {
	var targetOffset = $(this).offset().top;
	$('html,body').animate({ scrollTop: targetOffset }, 500);
}

/*
Function fills a select dropdown using a JSON object passed into it
*/
$.fn.fillSelect = function(data, preserveFirst) {
	return this.clearSelect(preserveFirst).each(function() {
		if (this.tagName == 'SELECT') {
			var dropdownList = this;
			$.each(data, function(index, optionData) {
				var option = new Option(optionData.Description, optionData.Id);
				(($.browser.msie) ? dropdownList.add(option) : dropdownList.add(option, null));
			});
		}
	});
}

$.fn.createPredeterminedAdditionalInfo = function(data) {
	var html = parseTemplate($("#itemtemplate").html(), { items: data });
	$(html).appendTo("#predetermined-additionalinfo");
}

$.fn.createAdditionalInfo = function(data) {
	this.html('');
	var html = parseTemplate($("#itemtemplate").html(), { items: data });
	try {
		$(html).appendTo("#additionalinformation");
	}
	catch (e) {} //Suppress Error message relating to no item
}


/*
Function clears a select dropdown using a JSON object passed into it
*/
$.fn.clearSelect = function(preserveFirst) {
	return this.each(function() {
		if (this.tagName == 'SELECT') {
			((preserveFirst == 1) ? this.options.length = 1 : this.options.length = 0);
		}
	});
}
/*
Function for Left
*/
$.left = function(textToSearch, n) {
	if (n <= 0)
		return "";
	else if (n > String(textToSearch).length)
		return str;
	else
		return String(textToSearch).substring(0, n);
}
/*
Function for Right
*/
$.right = function(textToSearch, n) {
	if (n <= 0)
		return "";
	else if (n > String(textToSearch).length)
		return str;
	else {
		var iLen = String(textToSearch).length;
		return String(textToSearch).substring(iLen, iLen - n);
	}
}
/*
Function for Mid
*/
$.mid = function(textToSearch, start, len) {
	if (start < 0 || len < 0) return "";
	var iEnd, iLen
	iLen = String(textToSearch).length;
	if (start + len > iLen)
		iEnd = iLen;
	else
		iEnd = start + len;
	return String(textToSearch).substring(start, iEnd);
}


var _tmplCache = {}
this.parseTemplate = function (str, data) {
	var err = "";
	try {
		var func = _tmplCache[str];
		if (!func) {
			var strFunc =
            "var p=[],print=function(){p.push.apply(p,arguments);};" +
                        "with(obj){p.push('" +
                str.replace(/[\r\t\n]/g, " ")
               .replace(/'(?=[^#]*#>)/g, "\t")
               .split("'").join("\\'")
               .split("\t").join("'")
               .replace(/<#=(.+?)#>/g, "',$1,'")
               .split("<#").join("');")
               .split("#>").join("p.push('")
               + "');}return p.join('');";
			//alert(strFunc);
			func = new Function("obj", strFunc);
			_tmplCache[str] = func;
		}
		return func(data);
	} catch (e) { err = e.message; }
	return "< # ERROR: " + err + " # >";
}

function FillMultipleSelects(movingFrom, movingTo) {
	var selected = $(movingFrom).val();
	if (selected == '' || selected == null) {
		return false; cr
	}
	var options = jQuery.merge(jQuery.makeArray($(movingTo + " option")), $(movingFrom + " option:selected")).sort(function(a, b) {
		return (a.text.toLowerCase() < b.text.toLowerCase()) ? -1 : 1;
	});
	var list = $(movingTo);
	var liOptions = '';
	$(movingFrom + " option:selected").remove();
	$(movingTo + " option").remove();
	jQuery.each(options, function(i, val) {
		liOptions += '<option selected="selected" value="' + options[i].value + '">' + options[i].text + '</option>';
	});
	list.append(liOptions);
	return false;
}

function simple_tooltip(target_items, name) {
	$(target_items).each(function(i) {
		if ($(this).attr("title") != '' && $(this).attr("class") == 'toolInfo') {
			$("body").append("<div class='" + name + "' id='" + name + i + "'><p>" + $(this).attr('title') + "</p></div>");
			var my_tooltip = $("#" + name + i);

			$(this).removeAttr("title").mouseover(function() {
				my_tooltip.css({ opacity: 0.8, display: "none" }).fadeIn(400);
			}).mousemove(function(kmouse) {
				my_tooltip.css({ left: kmouse.pageX + 15, top: kmouse.pageY + 15 });
			}).mouseout(function() {
				my_tooltip.fadeOut(400);
			});
		}
	});
}