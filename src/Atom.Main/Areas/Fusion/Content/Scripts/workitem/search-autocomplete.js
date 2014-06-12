var aSearch = "";
$(function() {
	//Auto-Complete
	ObtainSearchData();

	function ObtainSearchData() {
		$.postJSON("/Search/SearchAutoComplete", null, function(json) {
			$("#searchbox").autocomplete(json, {
				formatItem: function(item) {
					var result = (item.search + " <span style=\"float:right\">" + item.text + "</span>")
					return result;
				},
				multiple: true, autoFill: false, matchContains: false,
				multipleSeparator: ",",
				formatResult: function(row, i, n) {
					return row.search;
				},
				formatMatch: function(data, i, max) {
					return data.search;
				}
			})
			.result(function(event, data, formatted) {
				var currentValue = event.currentTarget.value;
				currentValue = $.left(currentValue, currentValue.length - 1);
				event.currentTarget.value = currentValue;
				$(event.currentTarget).setCursorPosition(currentValue.length);
			});
		},
		function(json, textStatus) {
			// fallback issues
			//alert('Error: Default searches returned');
			return [{ text: 'Open cases', search: 'status:open' }, { text: 'Closed cases', search: 's:closed' }, { text: 'In progress cases', search: 's:in progress'}];
		});
	}

	$("#searchbox").change(function() {
		aSearch = "";
	});

});