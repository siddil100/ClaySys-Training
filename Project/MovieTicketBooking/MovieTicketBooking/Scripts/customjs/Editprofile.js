function validateFirstName() {
    var firstNameInput = $('#FirstName');
    var firstName = firstNameInput.val().trim();
    var validationMessage = $('#errorFirstName');
    var isValid = true;

    var regex = /^[A-Za-z]{3,15}$/;

    if (!regex.test(firstName)) {
        validationMessage.text("First name must be 3-15 alphabetic characters, no spaces allowed.").show();
        isValid = false;
    } else {
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateLastName() {
    var lastNameInput = $('#LastName');
    var lastName = lastNameInput.val().trim();
    var validationMessage = $('#errorLastName');
    var isValid = true;

    var regex = /^[A-Za-z]{3,15}$/;

    if (!regex.test(lastName)) {
        validationMessage.text("Last name must be 3-15 alphabetic characters, no spaces allowed.").show();
        isValid = false;
    } else {
        validationMessage.text("").hide();
    }

    return isValid;
}

function validatePhoneNumber() {
    var phoneNumberInput = $('#PhoneNumber');
    var phoneNumber = phoneNumberInput.val().trim();
    var validationMessage = $('#errorPhoneNumber');
    var isValid = true;

    var regex = /^[6789]\d{9}$/;

    if (!regex.test(phoneNumber)) {
        validationMessage.text("Phone number must be 10 digits long, starting with 6, 7, 8, or 9.").show();
        isValid = false;
    } else {
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateAddress() {
    var addressInput = $('#Address');
    var address = addressInput.val().trim();
    var validationMessage = $('#errorAddress');
    var isValid = true;

    var regex = /^[A-Za-z0-9 ,.'-]{12,50}$/;

    if (!regex.test(address)) {
        validationMessage.text("Address must be between 12 and 50 characters, with allowed special characters.").show();
        isValid = false;
    } else {
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateDateOfBirth() {
    var dateOfBirthInput = $('#DateOfBirth');
    var dateOfBirth = new Date(dateOfBirthInput.val());
    var today = new Date();
    var minDate = new Date('1900-01-01');
    var eighteenYearsAgo = new Date(today.setFullYear(today.getFullYear() - 18));

    today = new Date(); // Reset today's date for further validations

    function formatDate(date) {
        var year = date.getFullYear();
        var month = ('0' + (date.getMonth() + 1)).slice(-2); // Months are zero-based
        var day = ('0' + date.getDate()).slice(-2);
        return `${year}-${month}-${day}`;
    }

    var isValid = !(dateOfBirth > eighteenYearsAgo || dateOfBirth < minDate);

    if (!isValid) {
        $('#errorDateOfBirth').text("Date of birth must be before today's date and ensure the user is at least 18 years old.").show();
    } else {
        $('#errorDateOfBirth').text("").hide();
    }

    return isValid;
}

$(document).ready(function () {
    // Set min and max attributes for the date input
    var dateOfBirthInput = $('#DateOfBirth');
    var today = new Date();
    var minDate = new Date('1900-01-01');
    var eighteenYearsAgo = new Date(today.setFullYear(today.getFullYear() - 18)); // Date at least 18 years

    dateOfBirthInput.attr('min', formatDate(minDate));
    dateOfBirthInput.attr('max', formatDate(eighteenYearsAgo));

    $('#editForm').on('submit', function (event) {
        var isFirstNameValid = validateFirstName();
        var isLastNameValid = validateLastName();
        var isPhoneNumberValid = validatePhoneNumber();
        var isAddressValid = validateAddress();
        var isDateOfBirthValid = validateDateOfBirth();

        if (!isFirstNameValid || !isLastNameValid || !isPhoneNumberValid || !isAddressValid || !isDateOfBirthValid) {
            event.preventDefault(); 
            alert("Please correct the errors before submitting.");
        }
    });
});




