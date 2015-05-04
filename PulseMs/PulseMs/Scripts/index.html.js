$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/GetView',
        data: { 'value': '1' },
        dataType: 'json',
        success: OnSuccess
    });

    $(document).on("keyup", "input[type='text']", function (e) {
        var targetId = e.target.id;
        $('p[id="' + targetId + '"]').empty();
        $('p[id="' + targetId + '"]').append($('input[id="' + targetId + '"]').val());
    });

    $(document).on("keyup", "input[type='datetime']", function (e) {
        var targetId = e.target.id;
        $('p[id="' + targetId + '"]').empty();
        $('p[id="' + targetId + '"]').append($('input[id="' + targetId + '"]').val());
    });

    $('#testList').change(function () {
        $.ajax({
            type: 'GET',
            url: '/Home/GetView',
            data: { 'value': $(this).val() },
            dataType: 'json',
            success: OnSuccess
        });
    });
});

function OnSuccess(response) {
    $('#replaceContent').empty();
    $('#replaceContent').append(response);
}