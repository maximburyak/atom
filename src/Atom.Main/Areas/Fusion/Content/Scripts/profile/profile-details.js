$(function() {

    $(document).ready(function() {
        bindDepartments();
    });

     $("#avatarupload,#signatureupload").MultiFile({
        max: 1,
        accept: 'gif,jpg,png,jpeg'
    });

    $("#saveavatar").click(function() {
        if ($("input[name='CurrentAvatar']:checked").length <= 0 && $("#avatarupload").val() == '') {
            alert('Please choose something to update');
            return false;
        }
        $("#profile").submit();
    });

    $("#savesignature").click(function() {
        if ($("input[name='CurrentSignature']:checked").length <= 0 && $("#signatureupload").val() == '') {
            alert('Please choose something to update');
            return false;
        }
        $("#profilesignature").submit();
    });

    $("#filterdelete").click(function() {
        if ($("#filterid").val() == '') {
            alert('Please choose something to delete');
            return false;
        }
        $("#profilefilters").submit();
        return false;
    });

    $("#defaultfilter").click(function() {
        if ($("#filterid").val() == '') {
            alert('Please choose something for your default');
            return false;
        }
        $("#filterdefault").val($("#filterid").val());
        $("#profiledefault").submit();
        return false;
    });

    $("#filterid").change(function() {
        if ($("option:selected[text$='[DEFAULT]']", this).length > 0) {
            $("#defaultfilter").hide();
            $("#defaultfilterdelete").show();
        }
        else {
            $("#defaultfilter").show();
            $("#defaultfilterdelete").hide();
        }
    });

    $("#assignedDepartment").change(function() {
        bindDepartments();
    });

    function bindDepartments() {
        var departmentid = $("#assignedDepartment > option:selected").attr("value");

        $.getJSON("/Fusion/Profile/FindDepartmentUsersByDepartmentId/" + departmentid,
            {},
            function(json) {
                if (json.length > 0) {
                    var options = '';
                    for (u in json) {
                        var user = json[u];
                        var selected = (user.IsAssignedToAuto) ? " selected = 'selected'" : '';
                        options += "<option" + selected + " value='" + user.Id + "'>" + user.Name + "</option>";
                    }
                    $("#assignToDepartmentUser").removeAttr('disabled').html(options);
                } else {
                    $("#assignToDepartmentUser").attr('disabled', true).html('');
                }
            },
			function(json) {
                $("#assignToDepartmentUser").attr('disabled', true).html('');
			}
		);
    }

    $("#defaultfilterdelete").click(function() {
        if ($("#filterid").val() == '') {
            alert('Please choose the default to remove');
            return false;
        }
        $("#filterremove").val($("#filterid").val());
        $("#profiledefaultremove").submit();
        return false;
    });

    $("#saveAutoAssign").click(function() {
        $("#assigneduser").val($("#assignToDepartmentUser").val());
        $("#profileautoassignto").submit();
        return false;
    });

});