$(function () {
    $.ajax({
        url: "/BasicData/GetColorForms",
        success: function (rtn) {
            if (rtn.succeeded) {
                var data = rtn.data;
                var html = '<option value="">所有成色</option>';
                for (var i = 0, len = data.length; i < len; i++) {
                    html += '<option value="' + data[i].value
                                 + '">' + data[i].text
                                 + '</option>';
                }
                $("#colorFormId").html(html);
            }
        },
        error: function () {
        }
    });
})