$(document).ready(function () {
    // When the user submits the Register form, prevent the default form submission and call the Register action method
    $("#exampleModalToggle2 form").submit(function (event) {
        event.preventDefault();
        $('#exampleModalToggle3').on('hidden.bs.modal', function (e) {
            $('.modal-backdrop').remove();
        });
        $('#exampleModalToggle').on('hidden.bs.modal', function (e) {
            $('.modal-backdrop').remove();
        });
        // Clear previous error messages
        $('.text-danger').empty();

        $.ajax({
            url: '/User/Register',
            type: 'POST',
            dataType: 'json',
            data: $(this).serialize(),
            success: function (response) {
                // If the registration was successful, display the Confirm modal
                if (response.success) {
                    // Hide the registration modal
                    $('#exampleModalToggle2').modal('hide');
                    // Show the confirmation modal
                    $("#exampleModalToggle3").modal("show");
                //    $("#exampleModalToggle3").modal("show");
                } else {
                    if (response.errors) {
                        // Update error messages based on response
                        $.each(response.errors, function (key, value) {
                            $('[name="' + key + '"]').siblings('.text-danger').text(value);
                        });
                    }
                    else {
                        // Display a general error message
                        $('#registrationErrors').text('Registration failed. Please try again later.');
                    }
                }
            },
            error: function () {
                $('#registrationErrors').text('An error occurred while processing your request.');
            }
        });
    });
});
