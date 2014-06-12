// Common functions
$(function () {

	$('#subscribeuser').change(function (e) {
		var $this = $(this);
		if ($this.val() != '' && confirm('Subscribe user: ' + $('option:selected', $this).text() + '?')) {
			$('#subscribe-user-form').submit();
		} else {
			e.stopPropagation();
		}
	});

});