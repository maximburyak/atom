$(function() {

	simple_tooltip("a", "tooltip");

	$('#addcomment').click(function() {
		var unitsOfWork = $('#UnitsOfWork').val();
		var msg = '';
		if (UnitsOfWorkRequired) {
			if (unitsOfWork == '') {
				msg = 'Please ensure units of work is completed.';
			}
		}
		if ($('#wmd-input').val() == '') {
			msg += "\nPlease ensure comment is completed."
		}

		if (msg != '') {
			alert(msg);
			return false;
		}
	});

	$('#assignto').change(function() {
		if ($('#assignto').val() != '') {
			$('#assign-form').submit();
		}
	});
	
	$('#assigntodept').change(function() {
		if ($('#assigntodept').val() != '') {
			$('#assigndept-form').submit();
		}
	});

	if (UnitsOfWorkRequired) {
		$('#UnitsOfWork').keypress(function(e) {
			var keycode = e.which;
//			var keypressed = String.fromCharCode(keycode)
//			var value = $(this).val();

			return (keycode > 47 && keycode < 58 || keycode == 8 || keycode == 0);
		});
	}

	$('#fileupload').MultiFile({
		max: 1,
		accept: 'gif,jpg,png,jpeg,doc,docx,pdf,gif,xls,xlsx,ppt,pps,vsd,mpp,msg,xml,txt,csv,zip'
	});

	$('#uploaddocument').click(function() {
		var val = $('#fileupload').val()
		if (val == '') {
			alert('Please enter a file to upload');
			return false;
		}
		$('#adddoc').submit();
		return false;
	});

	$('#CloseReason').change(function() {
		$('#rejectworkitem').attr("disabled", $(this).val() == '')
	});

	$('#rejectworkitem').click(function() {
		if ($('#CloseReason').val() != '') {
			$('#workitem-reject-form').submit();
		} return false;
	});

	$('#workitemapprove').click(function() {
		$('#workitem-approve-form').submit();
	});

	$("#emergencybutton")//.attr("disabled", $(":radio[name=emergencychange]:checked").length == 0)
				.click(function() {
					$("#emergencyform").submit();
					return false;
				});

	$(":radio[name=emergencychange]").click(function() {
		$("#emergencybutton").attr("disabled", false);
	});
});
		
		