function validateMovieName() {
    var movieNameInput = $('#MovieName');
    var movieName = movieNameInput.val().trim(); 
    var errorSpan = $('#errorMovieName');

    // 2 to 20 characters, letters or numbers, space allowed between, but not at start or end
    var regex = /^[A-Za-z0-9][A-Za-z0-9 ]{0,18}[A-Za-z0-9]$/;
    var isValid = true;

    if (!regex.test(movieName)) {
        errorSpan.text("Movie name must be 2-20 characters, containing only letters and numbers, with no leading or trailing spaces.").show();
        isValid = false;
    } else {
        errorSpan.text("").hide();
    }

    return isValid;
}

function validateDescription() {
    var descriptionInput = $('#Description');
    var description = descriptionInput.val().trim(); 
    var errorSpan = $('#errorDescription');

    //20 to 150 characters, allowing letters, numbers, spaces, commas, periods, apostrophes, hyphens, and exclamation marks
    var regex = /^[A-Za-z0-9 ,.'!@#()-]{20,150}$/;
    var isValid = true;

    if (!regex.test(description)) {
        errorSpan.text("Description must be 20-150 characters and can contain letters, numbers, spaces, commas, periods, apostrophes, hyphens, and exclamation marks.").show();
        isValid = false;
    } else {
        errorSpan.text("").hide();
    }

    return isValid;
}


function validateDuration() {
    var durationInput = $('#Duration');
    var duration = durationInput.val().trim(); 
    var errorSpan = $('#errorDuration');
    var isValid = true;

    var durationValue = parseInt(duration);

    if (isNaN(durationValue) || durationValue < 60 || durationValue > 300) {
        errorSpan.text("Duration must be an integer between 60 and 300 minutes.").show();
        isValid = false;
    } else {
        errorSpan.text("").hide();
    }

    return isValid;
}


function validateActor() {
    var actorInput = $('#Actor');
    var actor = actorInput.val().trim();
    var errorSpan = $('#errorActor');
    var isValid = true;

    // Regex: 3 to 20 characters, letters and spaces allowed, no leading or trailing spaces
    var regex = /^[A-Za-z][A-Za-z ]{1,18}[A-Za-z]$/;

    if (!regex.test(actor)) {
        errorSpan.text("Actor must be 3-20 characters long, contain only letters and spaces, with no leading or trailing spaces.").show();
        isValid = false;
    } else {
        errorSpan.text("").hide();
    }

    return isValid;
}

function validateActress() {
    var actressInput = $('#Actress');
    var actress = actressInput.val().trim();
    var errorSpan = $('#errorActress');
    var isValid = true;

    // Regex: 3 to 20 characters, letters and spaces allowed, no leading or trailing spaces
    var regex = /^[A-Za-z][A-Za-z ]{1,18}[A-Za-z]$/;

    if (!regex.test(actress)) {
        errorSpan.text("Actress must be 3-20 characters long, contain only letters and spaces, with no leading or trailing spaces.").show();
        isValid = false;
    } else {
        errorSpan.text("").hide();
    }

    return isValid;
}

function validateDirector() {
    var directorInput = $('#Director');
    var director = directorInput.val().trim();
    var errorSpan = $('#errorDirector');
    var isValid = true;

    // Regex: 3 to 20 characters, letters and spaces allowed, no leading or trailing spaces
    var regex = /^[A-Za-z][A-Za-z ]{1,18}[A-Za-z]$/;

    if (!regex.test(director)) {
        errorSpan.text("Director must be 3-20 characters long, contain only letters and spaces, with no leading or trailing spaces.").show();
        isValid = false;
    } else {
        errorSpan.text("").hide();
    }

    return isValid;
}





$('#editMovieForm').on('submit', function (event) {
    var isMovieNameValid = validateMovieName();
    var isDescriptionValid = validateDescription();
    var isDurationValid = validateDuration();
    var isActorValid = validateActor();
    var isActressValid = validateActress();
    var isDirectorValid = validateDirector();



    if (!isMovieNameValid || !isDescriptionValid || !isDurationValid || !isActorValid || !isActressValid || !isDirectorValid) {
        event.preventDefault(); // Prevent form submission
        alert("Please correct the errors before submitting.");
    }

});


$(document).ready(function () {
    var today = new Date();
    var fiveYearsAgo = new Date();
    fiveYearsAgo.setFullYear(today.getFullYear() - 5);

    var maxDate = today.toISOString().split('T')[0]; // Format: YYYY-MM-DD
    var minDate = fiveYearsAgo.toISOString().split('T')[0]; // Format: YYYY-MM-DD

    $('#ReleaseDate').attr('max', maxDate);
    $('#ReleaseDate').attr('min', minDate);
});
