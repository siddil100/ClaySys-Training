

$(document).ready(function () {
    // Get the URL from the data attribute
    var getCitiesUrl = $('#CityDropdown').data('get-cities-url');

    $('#StateDropdown').change(function () {
        var stateId = $(this).val();
        $.ajax({
            url: getCitiesUrl,
            data: { stateId: stateId },
            type: 'GET',
            success: function (data) {
                var cityDropdown = $('#CityDropdown');
                cityDropdown.empty();
                cityDropdown.append('<option value="">Select City</option>');
                $.each(data, function (index, item) {
                    cityDropdown.append('<option value="' + item.Value + '">' + item.Text + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.error('Error: ' + error);
            }
        });

    });



    $('#User_Email').blur(function () {
        var email = $(this).val();
        var emailErrorSpan = $(this).siblings('span.field-validation-valid');

        if (email) {
            $.ajax({
                url: checkEmailUrl,  // global variable
                type: 'GET',
                data: { email: email },
                success: function (data) {
                    if (!data) {
                        emailErrorSpan.text("This email is already registered. Please use a different email.").addClass("text-danger");
                    } else {
                        emailErrorSpan.text("").removeClass("text-danger");
                    }
                },
                error: function () {
                    emailErrorSpan.text("An error occurred while checking the email. Please try again.").addClass("text-danger");
                }
            });
        }
    });


});
