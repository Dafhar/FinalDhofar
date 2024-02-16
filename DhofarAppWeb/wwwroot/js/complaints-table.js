$(document).ready(function () {
    var table = $('#mycomplaints').DataTable({
        "columnDefs": [
            {
                "targets": [0],
                "className": "text-center text-nowrap p-3 side-border",
                "render": function (data, type, row) {
                    return type === 'display' ? '<span style="background-color:#5DD1B1;font-size:small;padding:7px;border-radius:15px; color:white;">' + data + '</span>' : data;
                }
            },
            {
                "targets": [1],
                "className": "text-center text-nowrap p-3",
                "render": function (data, type, row) {
                    return type === 'display' ? '<span style="font-size:medium;">' + data + '</span>' : data;
                }
            },
            { "targets": [2], "className": "text-center text-nowrap p-3" },
            {
                "targets": [3],
                "className": "text-center text-nowrap p-3",
                "render": function (data, type, row) {
                    return type === 'display' ? '<span style="color:#92929D;font-weight:light;">' + data + '</span>' : data;
                }
            },
            {
                "targets": [4],
                "className": "text-center text-nowrap p-3",
                "render": function (data, type, row) {
                    var backgroundColor = '';
                    if (data === 'جديد') {
                        backgroundColor = '#6EC1ED';
                    } else if (data === 'مغلق') {
                        backgroundColor = '#92929D';
                    } else if (data === 'قيد العمل') {
                        backgroundColor = '#5DD1B1';
                    }
                    return '<span class="ticket-status" style="font-size:small;padding:7px;border-radius:15px;color:white;background-color:' + backgroundColor + '">' + data + '</span>';
                }
            }
        ],
        "rowCallback": function (row, data, index) {
            $(row).addClass('d-flex justify-content-center align-items-center custom-row-spacing');
            // Add a margin-bottom to each row
            $(row).css('margin-top', '10px'); // Adjust the value as needed
        },
        "initComplete": function (settings, json) {
            $('#mycomplaints tbody').find('tr').addClass('row-spacing');
        },
        "lengthMenu": [[6, 10, 25, 50, -1], [6, 10, 25, 50, "All"]],
        "pageLength": 6,
        "createdRow": function (row) {
            $(row).addClass('row-spacing');
        }
    });
});
