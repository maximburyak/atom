$(function () {

	$("#channelAdd").click(function () {
		FillMultipleSelects("#ChannelsOut", "#Channels");
		return false;
	});

	$("#channelRemove").click(function () {
		FillMultipleSelects("#Channels", "#ChannelsOut");
		return false;
	});
	$("#insuranceCompanyAdd").click(function () {
		FillMultipleSelects("#InsuranceCompaniesOut", "#InsuranceCompanies");
		return false;
	});
	$("#insuranceCompanyRemove").click(function () {
		FillMultipleSelects("#InsuranceCompanies", "#InsuranceCompaniesOut");
		return false;
	});
	$("#supplierAdd").click(function () {
		FillMultipleSelects("#SuppliersOut", "#Suppliers");
		return false;
	});
	$("#supplierRemove").click(function () {
		FillMultipleSelects("#Suppliers", "#SuppliersOut");
		return false;
	});
	$("#productgroupsAdd").click(function () {
		FillMultipleSelects("#ProductGroupsOut", "#ProductGroups");
		return false;
	});
	$("#productgroupsRemove").click(function () {
		FillMultipleSelects("#ProductGroups", "#ProductGroupsOut");
		return false;
	});

	// DD Changes
	$("#ChannelsOut").change(function () {
		var value = $("#ChannelsOut").val();
		$("#channelAdd").attr("disabled", value == '' || value == null);
	});
	$("#Channels").change(function () {
		var value = $("#Channels").val();
		$("#channelRemove").attr("disabled", value == '' || value == null);
	});
	$("#InsuranceCompaniesOut").change(function () {
		var value = $("#InsuranceCompaniesOut").val();
		$("#insuranceCompanyAdd").attr("disabled", value == '' || value == null);
	});
	$("#InsuranceCompanies").change(function () {
		var value = $("#InsuranceCompanies").val();
		$("#insuranceCompanyRemove").attr("disabled", value == '' || value == null);
	});
	$("#SuppliersOut").change(function () {
		var value = $("#SuppliersOut").val();
		$("#supplierAdd").attr("disabled", value == '' || value == null);
	});
	$("#Suppliers").change(function () {
		var value = $("#Suppliers").val();
		$("#supplierRemove").attr("disabled", value == '' || value == null);
	});
	$("#ProductGroupsOut").change(function () {
		var value = $("#ProductGroupsOut").val();
		$("#productgroupsAdd").attr("disabled", value == '' || value == null);
	});
	$("#ProductGroups").change(function () {
		var value = $("#ProductGroups").val();
		$("#productgroupsRemove").attr("disabled", value == '' || value == null);
	});

	$("#ChannelsOut").click(function () {
		var value = $("#ChannelsOut").val();
		$("#channelAdd").attr("disabled", value == '' || value == null);
	});
	$("#Channels").click(function () {
		var value = $("#Channels").val();
		$("#channelRemove").attr("disabled", value == '' || value == null);
	});
	$("#InsuranceCompaniesOut").click(function () {
		var value = $("#InsuranceCompaniesOut").val();
		$("#insuranceCompanyAdd").attr("disabled", value == '' || value == null);
	});
	$("#InsuranceCompanies").click(function () {
		var value = $("#InsuranceCompanies").val();
		$("#insuranceCompanyRemove").attr("disabled", value == '' || value == null);
	});
	$("#SuppliersOut").click(function () {
		var value = $("#SuppliersOut").val();
		$("#supplierAdd").attr("disabled", value == '' || value == null);
	});
	$("#Suppliers").click(function () {
		var value = $("#Suppliers").val();
		$("#supplierRemove").attr("disabled", value == '' || value == null);
	});
	$("#ProductGroupsOut").click(function () {
		var value = $("#ProductGroupsOut").val();
		$("#productgroupsAdd").attr("disabled", value == '' || value == null);
	});
	$("#ProductGroups").click(function () {
		var value = $("#ProductGroups").val();
		$("#productgroupsRemove").attr("disabled", value == '' || value == null);
	});

	//dblclick ones too
	$("#ChannelsOut").dblclick(function () {
		FillMultipleSelects("#ChannelsOut", "#Channels");
		return false;
	});
	$("#Channels").dblclick(function () {
		FillMultipleSelects("#Channels", "#ChannelsOut");
		return false;
	});
	$("#InsuranceCompaniesOut").dblclick(function () {
		FillMultipleSelects("#InsuranceCompaniesOut", "#InsuranceCompanies");
		return false;
	});
	$("#InsuranceCompanies").dblclick(function () {
		FillMultipleSelects("#InsuranceCompanies", "#InsuranceCompaniesOut");
		return false;
	});
	$("#SuppliersOut").dblclick(function () {
		FillMultipleSelects("#SuppliersOut", "#Suppliers");
		return false;
	});
	$("#Suppliers").dblclick(function () {
		FillMultipleSelects("#Suppliers", "#SuppliersOut");
		return false;
	});
	$("#ProductGroupsOut").dblclick(function () {
		FillMultipleSelects("#ProductGroupsOut", "#ProductGroups");
		return false;
	});
	$("#ProductGroups").dblclick(function () {
		FillMultipleSelects("#ProductGroups", "#ProductGroupsOut");
		return false;
	});

	$("#createcrf").click(function () {
		$("#ProductGroups option").attr("selected", true);
		$("#Suppliers option").attr("selected", true);
		$("#InsuranceCompanies option").attr("selected", true);
		$("#Channels option").attr("selected", true);

		window.onbeforeunload = null;
		$(this).attr("disabled", true);
		$("#addform").submit();
	});

	$("#addform").bind('change keydown', function () {
		window.onbeforeunload = function () { return "You have chosen to cancel the creation of a CRF. Any completed information will be lost. Are you sure?"; }
		$("#createcrf").attr("disabled", false);
	});
});

function changeBoardMeeting() {
	var completionDate = $("#RequestedCompletionDate").val();
	var emergency = $(":radio[name='emergencychange']:checked").val()==1;
	if (completionDate != "") {
		var date = completionDate.split('/');
		var meetingdate = cbDate.split('/');
		if (date.length < 2) {
			return false;
		}
		var d = new Date(date[2], date[1] - 1, date[0], 0, 0, 0, 0);
		var md = new Date(meetingdate[2], meetingdate[1] - 1, meetingdate[0], 0, 0, 0, 0);
		var parsedValue = Date.parse(d);
		if (isNaN(parsedValue)) {
			return false;
		}
		else {
			parsedValue = new Date(parsedValue);
		}
		// Check for Date conflict
		if (parsedValue < md && (!emergency)) {
			alert('Please note: Your requested completion date is either before\nthe next CAB meeting or within a week following the next\nCAB meeting.\n\nYou must set the CRF to an Emergency Change');
			return false;
		}
	}
	return true;
}
