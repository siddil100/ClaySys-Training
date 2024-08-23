$(document).ready(function() {
    // Floating label for text-based inputs and textarea, check if value is present or not
    $('input:not([type="radio"]), textarea').each(function() {
        if ($(this).val()) {
            $(this).siblings('label').addClass('active');
        }
    });

    // Focus - when user clicks or types
    $('input:not([type="radio"]), textarea').on('focus', function() {
        $(this).siblings('label').addClass('active');
    });

    // Blur - when user leaves the textbox
    $('input:not([type="radio"]), textarea').on('blur', function() {
        if (!$(this).val()) {
            $(this).siblings('label').removeClass('active');
        }
    });

        //conditional dropdowm 
        const cityOptions = {
            kerala: [
                { value: "kottayam", text: "Kottayam" },
                { value: "thiruvananthapuram", text: "Thiruvananthapuram" }
            ],
            tamil_nadu: [
                { value: "kambam", text: "Kambam" },
                { value: "selam", text: "Selam" }
            ]
        };

        $('#state').change(function() {
            const selectedState = $(this).val();
            const cities = cityOptions[selectedState] || [];

            if (selectedState === "") {
                $('#city').prop('disabled', true);
            } else {
                $('#city').prop('disabled', false).empty().append('<option value="">Select City</option>');

                $.each(cities, function(index, city) {
                    $('#city').append($('<option>', {
                        value: city.value,
                        text: city.text
                    }));
                });
            }
        });
    

        $(function() {
            // Calculate the maximum date where the user would be 18 years old
            const today = new Date();
            const maxYear = today.getFullYear() - 18;
            const maxMonth = String(today.getMonth() + 1).padStart(2, '0');
            const maxDay = String(today.getDate()).padStart(2, '0');
            const maxDate = new Date(`${maxYear}-${maxMonth}-${maxDay}`);
        
            // Set the minimum date to January 1, 1960
            const minDate = new Date('1960-01-01');
        
            // Initialize the Datepicker with the min and max dates
            $('#dob').datepicker({
                dateFormat: 'yy-mm-dd',
                changeMonth: true,
                changeYear: true,
                yearRange: '1960:' + maxYear,
                minDate: minDate,
                maxDate: maxDate,
            });
        
            // Automatically set the label as active for Date of Birth
            $('#dob').datepicker().on('change', function() {
                if ($(this).val()) {
                    $(this).siblings('label').addClass('active');
                } else {
                    $(this).siblings('label').removeClass('active');
                }
            });
        });



        // validate first name and last name 
            function validateName(name) {
                const regex = /^[A-Za-z]{3,12}$/;
                return regex.test(name);
            }
    
            $('#firstname').on('input', function() {
                const firstname = $(this).val();
                if (!validateName(firstname)) {
                    $('#firstname-error').text('First name must be 3-12 letters with no spaces.').show();
                } else {
                    $('#firstname-error').hide();
                }
            });
    
            $('#lastname').on('input', function() {
                const lastname = $(this).val();
                if (!validateName(lastname)) {
                    $('#lastname-error').text('Last name must be 3-12 letters with no spaces.').show();
                } else {
                    $('#lastname-error').hide();
                }
            });
        

            // to check wether gender is selected 
            $('form').on('submit', function(e) {
                var genderSelected = $('input[name="gender"]:checked').length > 0;
                
                if (!genderSelected) {
                    e.preventDefault();
                    $('#gender-error').show();
                } else {
                    $('#gender-error').hide();
                }
            });

            // to check whether dob is selected
            $('form').on('submit', function(e) {
                var dob = $('#dob').val();
                
                if (!dob) {
                    e.preventDefault();
                    $('#dob-error').show();
                } else {
                    $('#dob-error').hide();
                }
            });

            // phone number validation 
            $('#phone').on('input', function() {
                var phone = $(this).val();
                var phonePattern = /^[6-9]\d{9}$/;
        
                if (!phonePattern.test(phone)) {
                    $('#phone-error').show();
                } else {
                    $('#phone-error').hide();
                }
            });


            // email validation 
            $('#email').on('input', function() {
                var email = $(this).val().trim(); // Trim leading and trailing spaces
            
                // Check for any spaces after trimming
                if (/\s/.test(email)) {
                    $('#emailError').show().text("No spaces allowed in the email address.");
                    $('#submit-button').prop('disabled', true); // Disable submit button if invalid
                } else {
                    var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            
                    // Validate email format without spaces
                    if (!emailPattern.test(email)) {
                        $('#emailError').show().text("Please enter a valid email address.");
                        $('#submit-button').prop('disabled', true); // Disable submit button if invalid
                    } else {
                        $('#emailError').hide();
                        $('#submit-button').prop('disabled', false); // Enable submit button if valid
                    }
                }
            });
            
            // Address validation 
            $('#address').on('input', function() {
                validateAddress();
            });
        
            function validateAddress() {
                var address = $('#address').val().trim();
                var errorMessage = '';
                
                // Regular expression to allow letters, numbers, spaces, commas, periods, apostrophes, and hyphens
                var addressRegex = /^[a-zA-Z0-9\s,.'-]*$/;
                // Regular expression to ensure at least one alphabet is present
                var alphabetRegex = /[a-zA-Z]/;
                
                // Check for invalid characters
                if (!addressRegex.test(address)) {
                    errorMessage = 'Address contains invalid characters.';
                } 
                // Check if address contains at least one alphabet
                else if (!alphabetRegex.test(address)) {
                    errorMessage = 'Address must contain at least one alphabet.';
                } 
                // Check if address is too short
                else if (address.length < 12) {
                    errorMessage = 'Address must be at least 12 characters long.';
                } 
                // Check if address is too long
                else if (address.length > 50) {
                    errorMessage = 'Address must be no more than 50 characters long.';
                }
            
                // Display or hide error message
                if (errorMessage) {
                    $('#address-error').text(errorMessage).show();
                } else {
                    $('#address-error').text('').hide();
                }
            }
            
            // function to validate username 
            function validateUsername() {
                var username = $('#username').val().trim();
                var isValid = true;
                var usernamePattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{3,12}$/;
            
                if (username === "") {
                    $('#usernameError').text("Username is required.").show();
                    isValid = false;
                } else if (!usernamePattern.test(username)) {
                    $('#usernameError').text("Username must be 3-12 characters, include one capital letter, one number, and one special symbol.").show();
                    isValid = false;
                } else {
                    $('#usernameError').hide();
                }
            
                return isValid;
            }
            // function to validate password 
            function validatePassword() {
                var username = $('#username').val().trim();
                var password = $('#password').val();
                var isValid = true;
                var passwordPattern = /^(?=.*[A-Z])(?=.*[\W_])(?=.*[a-z])[A-Za-z\d\W_]{6,}$/;
                
                if (password === "") {
                    $('#passwordError').text("Password is required.").show();
                    isValid = false;
                } else if (password === username) {
                    $('#passwordError').text("Password should not be the same as the username.").show();
                    isValid = false;
                } else if (!passwordPattern.test(password)) {
                    $('#passwordError').text("Password must be at least 6 characters long and include one capital letter, one special symbol, and one letter.").show();
                    isValid = false;
                } else {
                    $('#passwordError').hide();
                }
                
                return isValid;
            }
            // function to validate confirm-password
            function validateConfirmPassword() {
                var password = $('#password').val();
                var confirmPassword = $('#confirm-password').val();
                var isValid = true;
            
                if (confirmPassword === "") {
                    $('#confirmPasswordError').text("Please confirm your password.").show();
                    isValid = false;
                } else if (confirmPassword !== password) {
                    $('#confirmPasswordError').text("Passwords do not match.").show();
                    isValid = false;
                } else {
                    $('#confirmPasswordError').hide();
                }
            
                return isValid;
            }
            
            // Event listeners for individual field validation
            $('#username').on('input', function() {
                validateUsername();
            });
            
            $('#password').on('input', function() {
                validatePassword();
            });
            
            $('#confirm-password').on('input', function() {
                validateConfirmPassword();
            });
            
            // finally ensuring everything is validated on submit 
            $('form').on('submit', function(e) {
                let isValid = true;
            
                // Validate First Name
                if (!validateName($('#firstname').val())) {
                    $('#firstname-error').text('First name must be 3-12 letters with no spaces.').show();
                    isValid = false;
                } else {
                    $('#firstname-error').hide();
                }
            
                // Validate Last Name
                if (!validateName($('#lastname').val())) {
                    $('#lastname-error').text('Last name must be 3-12 letters with no spaces.').show();
                    isValid = false;
                } else {
                    $('#lastname-error').hide();
                }
            
                // Validate Gender
                if ($('input[name="gender"]:checked').length === 0) {
                    $('#gender-error').show();
                    isValid = false;
                } else {
                    $('#gender-error').hide();
                }
            
                // Validate Date of Birth
                if (!$('#dob').val()) {
                    $('#dob-error').show();
                    isValid = false;
                } else {
                    $('#dob-error').hide();
                }
            
                // Validate Phone Number
                const phonePattern = /^[6-9]\d{9}$/;
                if (!phonePattern.test($('#phone').val())) {
                    $('#phone-error').show();
                    isValid = false;
                } else {
                    $('#phone-error').hide();
                }
            
                // Validate Email
                const email = $('#email').val().trim();
                const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
                if (/\s/.test(email)) {
                    $('#emailError').text("No spaces allowed in the email address.").show();
                    isValid = false;
                } else if (!emailPattern.test(email)) {
                    $('#emailError').text("Please enter a valid email address.").show();
                    isValid = false;
                } else {
                    $('#emailError').hide();
                }
            
                // Validate Address
                var address = $('#address').val().trim();
                var addressRegex = /^[a-zA-Z0-9\s,.'-]*$/;
                if (!addressRegex.test(address) || address.length < 12 || address.length > 50) {
                    $('#address-error').text('Address must be 12-50 characters long and contain only valid characters.').show();
                    isValid = false;
                } else {
                    $('#address-error').hide();
                }
            
                // Validate Username
                if (!validateUsername()) {
                    isValid = false;
                }
            
                // Validate Password
                if (!validatePassword()) {
                    isValid = false;
                }
            
                // Validate Confirm Password
                if (!validateConfirmPassword()) {
                    isValid = false;
                }
            
                // Prevent form submission if any validation failed
                if (!isValid) {
                    e.preventDefault();
                }
                else{
                    alert("form is valid and submitted")
                }
            });



});
