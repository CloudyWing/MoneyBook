function createDateTimePicker() {
    $('input[type="text"].datePicker').datetimepicker({
        locale: 'zh-tw',
        format: "YYYY/MM/DD",
        showTodayButton: true
    });

    $('input[type="text"].dateTimePicker').datetimepicker({
        locale: 'zh-tw',
        format: "YYYY/MM/DD HH:mm",
        showTodayButton: true
    });
}

$(function () {
    createDateTimePicker();
});