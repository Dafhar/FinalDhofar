const fileInput = document.getElementById('fileInput');
fileInput.addEventListener('change', handleFileSelect);

let pdfCount = 0; // Counter for PDF files

// Specify the local path to pdf.worker.min.js


function handleFileSelect(event) {
    const files = event.target.files;
    const preview = document.getElementById('preview');
    const currentImagesLength = preview.getElementsByClassName('uploaded-image').length;

    if (currentImagesLength + files.length > 5) {
        Swal.fire({
            icon: "warning",
            title: "وصلت الى الحد الاقصى",
            text: "عذرا لا يمكنك رفع اكثر من 5 ملفات",
            confirmButtonColor: "#3CD3B6"
        });
        return;
    }

    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        if (file) {
            // Check file size
            if (file.size > 2 * 1024 * 1024) { // 2MB in bytes
                Swal.fire({
                    icon: "warning",
                    title: "حجم الملف كبير جدا",
                    text: "عذرا، لا يمكن رفع ملفات أكبر من 2 ميغابايت.",
                    confirmButtonColor: "#3CD3B6"
                });
                continue; // Skip processing this file
            }

            const reader = new FileReader();
            reader.onload = function (event) {
                const imageData = event.target.result;
                const div = document.createElement('div');
                div.classList.add('uploaded-image');

                if (file.type === 'application/pdf') {
                    // For PDF files, use the PDF icon
                    pdfCount++;
                    const pdfCover = document.createElement('div');
                    pdfCover.classList.add('pdf-cover');
                    pdfCover.innerHTML = `
                        <img src="assets/images/file-pdf-icon.svg" style="display:block; margin:auto;">
                        <p style="text-align:center;color: #11578F;font-size: x-small;padding-top: 7px;">ملف مرفق ${pdfCount}</p>
                    `;
                    div.appendChild(pdfCover);
                } else {
                    // For other images, use the uploaded image data
                    div.style.backgroundImage = `url('${imageData}')`;
                }
                div.style.backgroundSize = file.type === 'application/pdf' ? 'contain' : 'cover'; // Adjust the backgroundSize based on file type
                div.style.backgroundRepeat = 'no-repeat'; // Set backgroundRepeat to 'no-repeat'
                div.style.padding = '10px'; // Set your desired padding value

                const actions = document.createElement('div');
                actions.classList.add('actions');
                actions.innerHTML = '<i class="far fa-trash-alt delete-icon mx-1 text-white shadow-sm"></i>';
                actions.innerHTML += `<i class="far fa-eye mx-1 text-white shadow-sm" onclick="injectImage('${imageData}', '${file.type}')"></i>`;

                div.appendChild(actions);
                preview.appendChild(div);
            };

            if (file.type === 'application/pdf') {
                // If the file is a PDF, no need to read it as data URL
                reader.readAsArrayBuffer(file);
            } else {
                // For other images, read as data URL
                reader.readAsDataURL(file);
            }
        }
    }

    if (currentImagesLength + files.length > 5) {
        fileInput.disabled = true;
        Swal.fire({
            icon: "warning",
            title: "وصلت الى الحد الاقصى",
            text: "عذرا لا يمكنك رفع اكثر من 5 ملفات",
            confirmButtonColor: "#3CD3B6"
        });
    }
}



function injectImage(fileData, fileType, fileName) {
    if (fileType === 'application/pdf') {
        try {

            pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://mozilla.github.io/pdf.js/build/pdf.worker.js';

            pdfjsLib.GlobalWorkerOptions.timeout = 5000; // Set timeout to 5 seconds

            // Load the PDF document
            const loadingTask = pdfjsLib.getDocument({ data: fileData });

            // Render the first page
            loadingTask.promise.then(function(pdf) {
                // Fetch the first page
                pdf.getPage(1).then(function(page) {
                    // Set canvas dimensions to match the PDF page
                    const viewport = page.getViewport({ scale: 1.0 });
                    const canvas = document.createElement('canvas');
                    const context = canvas.getContext('2d');
                    canvas.height = viewport.height;
                    canvas.width = viewport.width;

                    // Render the page into the canvas
                    const renderContext = {
                        canvasContext: context,
                        viewport: viewport
                    };
                    page.render(renderContext).promise.then(function() {
                        // Open the canvas content in a new window
                        const url = canvas.toDataURL('image/png');
                        window.open(url, '_blank');
                    });
                });
            }).catch(function(error) {
                console.error('Error loading PDF:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Failed to load PDF',
                    text: 'There was an error loading the PDF document. Please try again later.',
                    confirmButtonColor: '#3CD3B6',
                });
            });
        } catch (error) {
            console.error('Error opening PDF:', error);
            Swal.fire({
                icon: 'error',
                title: 'Failed to open PDF',
                text: 'There was an error opening the PDF document. Please try again later.',
                confirmButtonColor: '#3CD3B6',
            });
        }
    } else {
        // For other images, use Swal to display the image
        Swal.fire({
            imageUrl: fileData,
            imageAlt: 'Image Preview',
            showCloseButton: true,
            confirmButtonColor: '#3CD3B6',
        });
    }
}


function displayPDF(pdfData) {
    // Display the PDF using pdf.js
    pdfjsLib.getDocument({ data: pdfData }).promise.then(function (pdfDoc) {
        // Fetch the first page
        return pdfDoc.getPage(1);
    }).then(function (page) {
        const pdfCanvas = document.createElement('canvas');
        const pdfContext = pdfCanvas.getContext('2d');

        // Set canvas dimensions to match the PDF page
        const viewport = page.getViewport({ scale: 1.5 });
        pdfCanvas.width = viewport.width;
        pdfCanvas.height = viewport.height;

        // Render PDF page to canvas
        const renderContext = {
            canvasContext: pdfContext,
            viewport: viewport
        };
        page.render(renderContext).promise.then(function () {
            // Open the PDF in a Swal modal
            Swal.fire({
                title: 'PDF Preview',
                html: pdfCanvas,
                showCloseButton: true,
                confirmButtonColor: '#3CD3B6',
            });
        });
    });
}

// Event delegation for dynamically added delete icons
document.getElementById('preview').addEventListener('click', function (event) {
    if (event.target.classList.contains('far') && event.target.classList.contains('fa-trash-alt')) {
        const imageDiv = event.target.closest('.uploaded-image');
        if (imageDiv) {
            Swal.fire({
                icon: "warning",
                title: "Are you sure?",
                text: "Once deleted, you will not be able to recover this image!",
                showCancelButton: true,
                confirmButtonColor: "#3CD3B6",
                cancelButtonColor: "gray",
                confirmButtonText: "Delete",
                dangerMode: true
            }).then((result) => {
                if (result.isConfirmed) {
                    imageDiv.remove(); // Remove the clicked image div
                    fileInput.disabled = false; // Enable the file input after removal
                }
            });
        }
    }
});

// Event delegation for dynamically added elements
document.getElementById('preview').addEventListener('mouseenter', function (event) {
    if (event.target.classList.contains('uploaded-image')) {
        const actions = event.target.querySelector('.actions');
        actions.style.display = 'flex';
        actions.style.cursor = 'pointer'; // Set cursor to pointer
    }
});

document.getElementById('preview').addEventListener('mouseleave', function (event) {
    if (event.target.classList.contains('uploaded-image')) {
        const actions = event.target.querySelector('.actions');
        actions.style.display = 'none';
    }
});





