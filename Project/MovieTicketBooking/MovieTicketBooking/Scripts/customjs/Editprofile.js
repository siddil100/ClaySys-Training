$(document).ready(function () {
    // Initial state: disable the submit button
    $('#buttonSubmit').prop('disabled', true);

    function validateFirstName() {
        var firstNameInput = $('#FirstName');
        var firstName = firstNameInput.val().trim();
        var validationMessage = $('#errorFirstName');
        var isValid = /^[A-Za-z]{3,15}$/.test(firstName);

        if (!isValid) {
            validationMessage.text("First name must be 3-15 alphabetic characters, no spaces allowed.").show();
        } else {
            validationMessage.text("").hide();
        }

        return isValid;
    }

    function validateLastName() {
        var lastNameInput = $('#LastName');
        var lastName = lastNameInput.val().trim();
        var validationMessage = $('#errorLastName');
        var isValid = /^[A-Za-z]{3,15}$/.test(lastName);

        if (!isValid) {
            validationMessage.text("Last name must be 3-15 alphabetic characters, no spaces allowed.").show();
        } else {
            validationMessage.text("").hide();
        }

        return isValid;
    }

    function validatePhoneNumber() {
        var phoneNumberInput = $('#PhoneNumber');
        var phoneNumber = phoneNumberInput.val().trim();
        var validationMessage = $('#errorPhoneNumber');
        var isValid = /^[6789]\d{9}$/.test(phoneNumber);

        if (!isValid) {
            validationMessage.text("Phone number must be 10 digits long, starting with 6, 7, 8, or 9.").show();
        } else {
            validationMessage.text("").hide();
        }

        return isValid;
    }

    function validateAddress() {
        var addressInput = $('#Address');
        var address = addressInput.val().trim();
        var validationMessage = $('#errorAddress');
        var isValid = /^[A-Za-z0-9 ,.'-]{12,50}$/.test(address);

        if (!isValid) {
            validationMessage.text("Address must be between 12 and 50 characters, with allowed special characters.").show();
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
            $('#errorDateOfBirth').text("Date of birth must ensure the user is at least 18 years old.").show();
        } else {
            $('#errorDateOfBirth').text("").hide();
        }

        return isValid;
    }

    function checkFormValidity() {
        var isFormValid = validateFirstName() &&
            validateLastName() &&
            validatePhoneNumber() &&
            validateAddress() &&
            validateDateOfBirth();

        $('#buttonSubmit').prop('disabled', !isFormValid);
        return isFormValid;
    }

    $('#FirstName').on('input', checkFormValidity);
    $('#LastName').on('input', checkFormValidity);
    $('#PhoneNumber').on('input', checkFormValidity);
    $('#Address').on('input', checkFormValidity);
    $('#DateOfBirth').on('change', checkFormValidity);

    $('#editForm').on('submit', function (event) {
        if (!checkFormValidity()) {
            event.preventDefault();
            alert("Please correct the errors before submitting.");
        }
    });
});
