
document.addEventListener("DOMContentLoaded", function() {
    const myModal = document.getElementById('myModal');
    const myInput = document.getElementById('myInput');

    if (myModal && myInput) {
        myModal.addEventListener('shown.bs.modal', () => {
            myInput.focus();
        });
    } else {
        console.error("One or both of the elements 'myModal' and 'myInput' not found.");
    }

    // Your other code here...
});


// validation
var form = document.getElementById('loginForm');

// Handle form submission
form.addEventListener('submit', function(event) {
    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
    }

    form.classList.add('was-validated');
}, false);



function togglePassword(passwordId) 
{ 
    var passwordInput = document.getElementById(passwordId); 
    var eyeIconId = "eyeIcon" + passwordId.slice(-1); 
    var eyeIcon = document.getElementById(eyeIconId);

if (passwordInput.type === "password") {
    passwordInput.type = "text";
    eyeIcon.classList.remove("fa-eye");
    eyeIcon.classList.add("fa-eye-slash");
} else {
    passwordInput.type = "password";
    eyeIcon.classList.remove("fa-eye-slash");
    eyeIcon.classList.add("fa-eye");
}
}


// page links

function goToEditProfile() {
    window.location.href = 'edit-profile.html';
  }

  function goToHome() {
    window.location.href = 'home.html';
  }

  function goToComplaints() {
    window.location.href = 'complaints.html';
  }

// dropdown.js

$(document).ready(function() {
    console.log("Dropdown script loaded.");

    // Add more items to dropdown here.
    var addedItems = ["Billie Joe", "Mike Dirnt", "Tre Cool", "dua", "Billie Joe", "Mike Dirnt", "Tre Cool", "dua"];

    // When see more is clicked this method is called.
    $('#seeMore').click(function(event) {
        event.preventDefault(); // Prevent default link behavior

        console.log("See more clicked.");

        for (var i = addedItems.length - 1; i >= 0; i--) {
            // This removes the "See more" item on the list
            $('#seeMore').parent().remove();
            // Appending the new items on the list
            $('#commentList').append(
                $('<li>').append(
                    $('<a>').attr('href', '#').addClass('dropdown-item text-end arrow-item').attr('role', 'menuitem').attr('tabindex', '-1').append(
                        $('<span>').attr('class', 'px-2').append(
                            $('<img>').attr('src', 'assets/images/chat-text.svg')
                        ),
                        addedItems[i]
                    )
                )
            );
        }

        console.log("New items added.");

         // Remove dropdown-divider
         $('.dropdown-divider').remove();

        // Check if the number of items exceeds 10
        if ($('#commentList li').length > 10) {
            // Apply scrolling
            $('#commentList').addClass('scrollable');
        }

        // Re-open the dropdown after a short delay
        setTimeout(function() {
            $('#dropdownMenu1').trigger('click.bs.dropdown');
        }, 100);
    });
});


$(document).ready(function() {
    console.log("Dropdown script loaded.");

    // Add more items to dropdown here.
    var addedItems = ["Billie Joe", "Mike Dirnt", "Tre Cool", "dua", "Billie Joe", "Mike Dirnt", "Tre Cool", "dua"];

    // When see more is clicked this method is called.
    $('#seeMore2').click(function(event) {
        event.preventDefault(); // Prevent default link behavior

        console.log("See more clicked.");

        for (var i = addedItems.length - 1; i >= 0; i--) {
            // Appending the new items on the list
            $('#notificationList').append(
                $('<li>').append(
                    $('<a>').attr('href', '#').addClass('dropdown-item text-end arrow-item').attr('role', 'menuitem').attr('tabindex', '-1').append(
                        $('<span>').attr('class', 'px-2').append(
                            $('<img>').attr('src', 'assets/images/chat-text.svg')
                        ),
                        addedItems[i]
                    )
                )
            );
        }

        console.log("New items added.");

        // Remove the "See more" link
        $('#seeMore2').parent().remove();

        // Remove dropdown-divider if exists
        $('.dropdown-divider').remove();

        // Check if the number of items exceeds 10
        if ($('#notificationList li').length > 10) {
            // Apply scrolling
            $('#notificationList').addClass('scrollable');
        }

        // Re-open the dropdown after a short delay
        setTimeout(function() {
            $('#dropdownMenu2').trigger('click.bs.dropdown');
        }, 100);
    });
});

