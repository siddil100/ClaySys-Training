$(document).ready(function () {
    // Date and Time Validation
    var today = new Date();
    var tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 2); 

    var tenDaysFromTomorrow = new Date();
    tenDaysFromTomorrow.setDate(tomorrow.getDate() + 10); // 10 days from day after tommorrow

    var minDate = tomorrow.toISOString().split('T')[0]; // Format: YYYY-MM-DD
    var maxDate = tenDaysFromTomorrow.toISOString().split('T')[0]; // Format: YYYY-MM-DD

    $('#ShowDate').attr('min', minDate);
    $('#ShowDate').attr('max', maxDate);

    // Start Time Validation
    $('#StartTime').on('change', function () {
        var startTime = $(this).val();
        var hours = parseInt(startTime.split(':')[0], 10);

        // Disable invalid start times
        if (startTime === "00:00" || (hours >= 0 && hours < 4)) {
            $(this).val(""); // Reset to empty
            $('#errorStartTime').text("Start time cannot be 00:00 and hours from 12:00 to 03:59 am are not allowed.").show();
        } else {
            $('#errorStartTime').text("").hide();
        }
    });

    // Form Submission Validation
    $('#showtimeForm').on('submit', function (event) {
        var isMovieIdValid = $('#MovieId').val() !== "";
        var isShowDateValid = $('#ShowDate').val() !== "";
        var isStartTimeValid = $('#StartTime').val() !== "" && $('#StartTime').val() !== "00:00";

        if (!isMovieIdValid) {
            $('#errorMovieId').text("Please select a movie.").show();
        } else {
            $('#errorMovieId').text("").hide();
        }

        if (!isShowDateValid) {
            $('#errorShowDate').text("Please select a valid show date.").show();
        } else {
            $('#errorShowDate').text("").hide();
        }

        if (!isStartTimeValid) {
            $('#errorStartTime').text("Please select a valid start time between 04:00 am and 12:00.").show();
        } else {
            $('#errorStartTime').text("").hide();
        }

        if (!isMovieIdValid || !isShowDateValid || !isStartTimeValid) {
            event.preventDefault();
            alert("Please correct the errors before submitting.");
        }
    });
});
