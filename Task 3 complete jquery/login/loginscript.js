$(document).ready(function() {
    // Floating label,check value is present or not if yes add active class else remove it
    $('input').each(function() {
        if ($(this).val()) {
            $(this).siblings('label').addClass('active');
        }
    });
    //   focus- when user clicks or typing 
    $('input').on('focus', function() {
        $(this).siblings('label').addClass('active');
    });
    //   blur-when user leaves the textbox 
    $('input').on('blur', function() {
        if (!$(this).val()) {
            $(this).siblings('label').removeClass('active');
        }
    });

    // Form validation
    $('#loginForm').on('submit', function(e) {
        let isValid = true;

        // Validate email
        const email = $('#email').val().trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (email === '' || !emailRegex.test(email)) {
            $('#emailError').text('Please enter a valid email address.');
            isValid = false;
        } else {
            $('#emailError').text('');
        }

        // Validate password
        const password = $('#password').val().trim();
        if (password === '') {
            $('#passwordError').text('Please enter your password.');
            isValid = false;
        } else if (password.length < 6) {
            $('#passwordError').text('Password must be at least 6 characters long.');
            isValid = false;
        } else {
            $('#passwordError').text('');
        }

        if (!isValid) {
            e.preventDefault(); // Prevent form submission if validation fails
            console.log('Form is invalid. Preventing submission.');
        } else {
            alert("Form submitted")
        }
    });
});