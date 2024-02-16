

// Function to toggle bookmark icon
function toggleBookmark() {
    var icon = document.getElementById('bookmarkIcon');
    
    // Toggle the solid and regular classes
    icon.classList.toggle('fa-solid');
    icon.classList.toggle('fa-regular');
}

// Attach click event listener to the icon
document.getElementById('bookmarkIcon').addEventListener('click', toggleBookmark);
