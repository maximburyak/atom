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
