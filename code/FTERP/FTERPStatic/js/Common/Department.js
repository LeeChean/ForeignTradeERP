function GetSubDepartment(obj) {
    var id = "Dep_" + (parseInt($(obj).attr("id").split('_')[1]) + 1);

    if ($(obj).val() != "") {
        $("#DepartmentId").val($(obj).val());
    }
    else {
        parent = "#Dep_" + (parseInt($(obj).attr("id").split('_')[1]) - 1)
        $("#DepartmentId").val($(parent).val());
    }

    $.post("/Home/User/SubDepartment?id=" + id + "&depId=" + $(obj).val(), function (data) {
        if (null != data && data != "") {
            var parent = $(obj).parent();
            $(obj).nextAll().remove();
            parent.append(" " + $.trim(data));
        }
    });
}