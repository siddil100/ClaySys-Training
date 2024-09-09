function validateGender() {
    var isGenderSelected = $('input[name="UserDetails.Gender"]:checked').length > 0;
    var validationMessage = $('#genderError');
    var isValid = true;

    if (!isGenderSelected) {
        validationMessage.text("Please select a gender.").show();
        isValid = false;
    } else {
        validationMessage.text("").hide();
    }

    return isValid;
}


function validatePhoneNumber() {
    var phoneNumberInput = $('#UserDetails_PhoneNumber');
    var phoneNumber = phoneNumberInput.val().trim();
    var validationMessage = phoneNumberInput.siblings('span.text-danger');
    var isValid = true;

    var regex = /^[6-9]\d{9}$/; //Phone number starting with 6, 7, 8, or 9, and exactly 10 digits.

    if (!regex.test(phoneNumber)) {
        phoneNumberInput.css('border', '2px solid red');
        validationMessage.text("Phone number must be exactly 10 digits and start with 6, 7, 8, or 9.").show();
        isValid = false;
    } else {
        phoneNumberInput.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateAddress() {
    var addressInput = $('#UserDetails_Address');
    var address = addressInput.val().trim();
    var validationMessage = addressInput.siblings('span.text-danger');
    var isValid = true;

    //Alphabets, numbers, spaces, commas, periods, apostrophes, and hyphens
    var regex = /^[A-Za-z0-9\s,.'-]{10,50}$/;

   
    var containsAlphabet = /[A-Za-z]/.test(address);
    var containsValidChar = /[A-Za-z0-9\s,.'-]/.test(address);

    if (!regex.test(address) || !containsAlphabet) {
        addressInput.css('border', '2px solid red');
        validationMessage.text("Address must be 10-50 characters and contain letters.").show();
        isValid = false;
    } else {
        addressInput.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateState() {
    var stateDropdown = $('#StateDropdown');
    var state = stateDropdown.val();
    var validationMessage = stateDropdown.siblings('span.text-danger');
    var isValid = true;

    if (!state || state === "0") { 
        stateDropdown.css('border', '2px solid red');
        validationMessage.text("Please select a state.").show();
        isValid = false;
    } else {
        stateDropdown.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateCity() {
    var cityDropdown = $('#CityDropdown');
    var city = cityDropdown.val();
    var validationMessage = cityDropdown.siblings('span.text-danger');
    var isValid = true;

    if (!city || city === "0") { 
        cityDropdown.css('border', '2px solid red');
        validationMessage.text("Please select a city.").show();
        isValid = false;
    } else {
        cityDropdown.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}


function validateEmail() {
    var emailInput = $('#User_Email');
    var email = emailInput.val().trim();
    var emailErrorSpan = emailInput.siblings('span.text-danger');
    var isValid = true;

    // Clear previous error messages
    emailErrorSpan.text("").removeClass("text-danger");

    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailRegex.test(email)) {
        emailInput.css('border', '2px solid red');
        emailErrorSpan.text("Please enter a valid email address.").addClass("text-danger");
        isValid = false;
    } else {
        
        $.ajax({
            url: checkEmailUrl,  // Use the global variable
            type: 'GET',
            data: { email: email },
            async: false,
            success: function (data) {
                if (!data) {
                    emailErrorSpan.text("This email is already registered. Please use a different email.").addClass("text-danger");
                    emailInput.css('border', '2px solid red');
                    isValid = false;
                } else {
                    emailErrorSpan.text("").removeClass("text-danger");
                    emailInput.css('border', '2px solid green');
                }
            },
            error: function () {
                emailErrorSpan.text("An error occurred while checking the email. Please try again.").addClass("text-danger");
                emailInput.css('border', '2px solid red');
                isValid = false;
            }
        });
    }

    return isValid;
}


function validateFirstName() {
    var firstNameInput = $('#UserDetails_FirstName');
    var firstName = firstNameInput.val().trim();
    var validationMessage = firstNameInput.siblings('span.text-danger');
    var isValid = true;

    var regex = /^[A-Za-z]{3,15}$/;

    if (!regex.test(firstName)) {
        firstNameInput.css('border', '2px solid red');
        validationMessage.text("First name must be 3-15 alphabetic characters, no spaces allowed.").show();
        isValid = false;
    } else {
        firstNameInput.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateLastName() {
    var lastNameInput = $('#UserDetails_LastName');
    var lastName = lastNameInput.val().trim();
    var validationMessage = lastNameInput.siblings('span.text-danger');
    var isValid = true;

    var regex = /^[A-Za-z]{3,15}$/;

    if (!regex.test(lastName)) {
        lastNameInput.css('border', '2px solid red');
        validationMessage.text("Last name must be 3-15 alphabetic characters, no spaces allowed.").show();
        isValid = false;
    } else {
        lastNameInput.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}

function validateDOB() {
    var dobInput = $('#UserDetails_Dob');
    var dob = dobInput.val().trim();
    var validationMessage = dobInput.siblings('span.text-danger');
    var isValid = true;

    var today = new Date();
    var eighteenYearsAgo = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());

    if (!dob) {
       
        validationMessage.text("Date of birth is required.").show();
        isValid = false;
    } else {
        var dobDate = new Date(dob);
        if (dobDate > eighteenYearsAgo) {
            dobInput.css('border', '2px solid red');
            validationMessage.text("You must be at least 18 years old.").show();
            isValid = false;
        } else {
            dobInput.css('border', '2px solid green');
            validationMessage.text("").hide();
        }
    }

    return isValid;
}

function validatePassword() {
    var passwordInput = $('#User_Password');
    var password = passwordInput.val().trim();
    var passwordErrorSpan = passwordInput.siblings('span.text-danger');
    var isValid = true;

    // Clear previous error messages
    passwordErrorSpan.text("").removeClass("text-danger");

    // Regular expression for password validation
    var passwordRegex = /^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,12}$/;

    if (!passwordRegex.test(password)) {
        passwordInput.css('border', '2px solid red');
        passwordErrorSpan.text("Password must be 6-12 characters long, including at least one letter, one number, and one special symbol.").addClass("text-danger");
        isValid = false;
    } else {
        passwordInput.css('border', '2px solid green');
        passwordErrorSpan.text("").removeClass("text-danger");
    }

    return isValid;
}
function validateConfirmPassword() {
    var confirmPasswordInput = $('#ConfirmPassword');
    var passwordInput = $('#User_Password');
    var confirmPassword = confirmPasswordInput.val().trim();
    var password = passwordInput.val().trim();
    var validationMessage = $('#ConfirmPasswordError');
    var isValid = true;

    if (confirmPassword !== password) {
        confirmPasswordInput.css('border', '2px solid red');
        validationMessage.text("Passwords do not match.").show();
        isValid = false;
    } else {
        confirmPasswordInput.css('border', '2px solid green');
        validationMessage.text("").hide();
    }

    return isValid;
}

$(document).ready(function () {
    setDatepickerRestrictions(); 
    

    // Form submit handler
    $('#signupForm').on('submit', function (event) {
        var isFirstNameValid = validateFirstName();
        var isLastNameValid = validateLastName();
        var isConfirmPasswordValid = validateConfirmPassword();
        var isDOBValid = validateDOB();
        var isGenderValid = validateGender();
        var isPhoneNumberValid = validatePhoneNumber();
        var isAddressValid = validateAddress();
        var isStateValid = validateState();
        var isCityValid = validateCity();
        var isEmailValid = validateEmail();
        var isPasswordValid = validatePassword();

        if (!isFirstNameValid || !isLastNameValid || !isConfirmPasswordValid || !isDOBValid || !isGenderValid || !isPhoneNumberValid || !isAddressValid || !isStateValid || !isCityValid || !isEmailValid || !isPasswordValid) {
            event.preventDefault(); // Prevent form submission
            alert("Please correct the errors before submitting.");
        }
    });
});




function setDatepickerRestrictions() {
    var today = new Date();
    var eighteenYearsAgo = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());

    $('#UserDetails_Dob').datepicker({
        dateFormat: 'yy-mm-dd',
        minDate: new Date(1970, 0, 1), // Allow selection from January 1, 1970
        maxDate: today, // Disable future dates beyond today
        beforeShowDay: function (date) {
            return [date <= eighteenYearsAgo, ""];
        },
        changeMonth: true,
        changeYear: true,
        yearRange: "1970:" + today.getFullYear(), // Allow years from 1970 to current year
        defaultDate: eighteenYearsAgo // Set default date to 18 years ago
    });
}
