$(function () {

	$("#saveUnLinkedItems").click(function () {
		$("#linkedworkitems-form").submit();
		return false;
	});

	$('.caseitem').live('click', function (e) {
		var $this = $(this);
		var $context = $this.parents('form:first');
		var item = $this.find(':checkbox:first')[0];
		item.checked = !item.checked;
		validateform($context);
		e.stopPropagation();
	});

	$('.chk').live('click', function (e) {
		validateform($(this).parents('form:first'));
		e.stopPropagation();
	});

	validateform = function ($context) {
		var disableButtons = (getcheckeditems($context).length == 0);
		$('input[type="button"]', $context).attr('disabled', disableButtons);
	};

	$('#linksearchbox').keypress(function (e) {
		if (e.keyCode == 13) {
			GetLinkResults();
			e.preventDefault();
			e.stopPropagation();
			return false;
		}
		
	});

	getcheckeditems = function ($context) {
		return $('.chk:checked', $context);
	}
});

function GetLinkResults() {
	var linksearchbox = $("#linksearchbox").val();
	if (linksearchbox != "") {
		$('#detailscontainer').slideUp(function () {
			$.post(url, { linksearch: linksearchbox,
				workitemidtolinkto: workitemidtolinkto
			}, function (data) {
				$('#details-results').html(data);
				$('#detailscontainer').slideDown();
			});
		});
	}
}