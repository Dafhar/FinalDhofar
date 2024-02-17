$(document).ready(function () {
    // When the user submits the Register form, prevent the default form submission and call the Register action method
    $("#exampleModalToggle2 form").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: '/User/Register',
            type: 'POST',
            dataType: 'json',
            data: $(this).serialize(),
            success: function (response) {
                // If the registration was successful, display the Confirm modal
                if (response.success) {
                    $("#exampleModalToggle3").modal("show");
                } else {
                    if (response.errors) {

                        $('.text-danger').empty();
                        $.each(response.errors, function (key, value) {
                            if (key === 0) {
                                $('[name="Email"]').siblings('.text-danger').text(value);

                                
                            }
                            else if (key === 1) {
                                $('[name="Fullname"]').siblings('.text-danger').text(value);

                                

                            }
                            else if (key === 2) {
                                $('[name="Password"]').siblings('.text-danger').text(value);

                                

                            }
                            else if (key === 3) {
                                $('[name="UserName"]').siblings('.text-danger').text(value);

                                

                            }
                            else if (key === 4) {
                                $('[name="PhoneNumber"]').siblings('.text-danger').text(value);

                                

                            }
                            else if (key === 5) {
                                $('[name="ConfirmPassword"]').siblings('.text-danger').text(value);

                                

                            }

                        });
                    }
                    // If there were errors, display them in the Join Us modal
                    
                }
            },
            error: function () {
                alert('An error occurred while processing your request.');
            }
        });
    });   
});