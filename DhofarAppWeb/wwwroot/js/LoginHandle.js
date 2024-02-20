$(document).ready(function () {
    $('#exampleModalToggle form').submit(function (e) {
        e.preventDefault(); // Prevent the form from submitting normally

        $('#exampleModalToggle3').on('hidden.bs.modal', function (e) {
            $('.modal-backdrop').remove();
        });

        $('#exampleModalToggle2').on('hidden.bs.modal', function (e) {
            $('.modal-backdrop').remove();
        });

        // Clear previous error messages
        $('.text-danger').empty();
        $('#loginErrors').empty(); // Clear login errors

        $.ajax({
            url: '/User/Login',
            type: 'POST',
            dataType: 'json',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    $('#exampleModalToggle').modal('hide');
                    $('#exampleModalToggle3').modal('show');
                }
                else if (response.errors && response.errors.hasOwnProperty("InvalidLogin")) {
                    $('#loginErrors').text('Invalid login. Please try again.');

                } 
                else if (response.errors) {
                    $.each(response.errors, function (key, value) {
                        $('[name="' + key + '"]').siblings('.text-danger').text(value);
                    });
                }
               else {
                    $('#loginErrors').text('Login failed. Please try again later.');
                }

            },
            error: function () {
                // Handle AJAX errors
                alert('An error occurred while processing your request.');
            }
        });
    });

});
