$(document).ready(function () {
    $('#exampleModalToggle form').submit(function (e) {
        e.preventDefault(); // Prevent the form from submitting normally

        $.ajax({
            url: '/User/Login',
            type: 'POST',
            dataType: 'json',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    $('#exampleModalToggle').modal('hide');
                    $('#exampleModalToggle3').modal('show');
                } else {
                    if (response.errors) {
                        // Clear previous error messages
                        $('.text-danger').empty();

                        // Display validation errors in the form
                        $.each(response.errors, function (key, value) {
                            if (key === 0) {
                                $('[name="Password"]').siblings('.text-danger').text(value);
                                console.log(key);
                            } else if (key === 1) {
                                $('[name="PhoneNumber"]').siblings('.text-danger').text(value);
                                console.log(key);
                            }
                        });
                    }

                }
            },
            error: function () {
                // Handle AJAX errors
                alert('An error occurred while processing your request.');
            }
        });
    });
});