$(function () {
    // Clicking the checkbox toggles the item Done
    $(document).on('change', '.task-done', function () {
        var id = ($(this).parent().parent().parent().attr('id'));

        var toDoRow = ($(this).parent().parent().parent());

        $.post('ToDo/QuickComplete/' + id, function (data) {
            var result = data.success
            var done = data.done == "true" ? true : false;
            var completedDateTime = data.completedDateTime != null ? data.completedDateTime : '';
            var overdue = data.overdue == "true" ? true : false;

            toDoRow.find('.td-completed').html(completedDateTime);
            if (done) {
                toDoRow.find('.no-strikethrough').removeClass('no-strikethrough').addClass('done');
                toDoRow.removeClass('danger');
            }
            else {
                toDoRow.find('.done').removeClass('done').addClass('no-strikethrough');
                if (overdue) {
                    toDoRow.addClass('danger');
                }
            }

            $('.alert').hide();

        });

    });

    // Clicking on row shows details page
    $(document).on('click', '.details', function () {
        var id = ($(this).parent().attr('id'));
        window.location.replace('/ToDo/Details/' + id);
    });
});