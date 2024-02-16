$(document).ready(function () {
    var table = $('#mytopics').DataTable({
        "columnDefs": [
            {
                "targets": [1,4],
                "className": "text-center text-nowrap p-3",
                "render": function (data, type, row) {
                    return type === 'display' ? '<span style="">' + data + '</span>' : data;
                }
            },
            { "targets": [2], "className": "text-center text-nowrap px-5 p-3" },
            {
                "targets": [3],
                "className": "text-center text-nowrap px-3",
                "render": function (data, type, row) {
                    return type === 'display' ? '<span style="color:#92929D;font-weight:light;">' + data + '</span>' : data;
                }
            },
            {
                "targets": [0],
                "className": "text-center text-nowrap p-3 side-border ",
                "render": function (data, type, row) {
                    var backgroundColor = '';
                    if (data === 'الأفكار المبتكرة') {
                        backgroundColor = '#F45B3D';
                    } else if (data === 'نقطة نقاش') {
                        backgroundColor = '#3CD3B6';
                    } else if (data === 'البيئة') {
                        backgroundColor = '#2BA899';
                    } else if (data === 'تطوير البنى التحتية') {
                        backgroundColor = '#11578F';
                    } else if (data === 'المقترحات') {
                        backgroundColor = '#6FC1EB';
                    }
                    
                    return '<span class="ticket-status" style="font-size:small;padding:7px;border-radius:15px;color:white;background-color:' + backgroundColor + '">' + data + '</span>';
                }
            }
        ],
        "rowCallback": function (row, data, index) {
            $(row).addClass('d-flex justify-content-center align-items-center custom-row-spacing');
        },
        "rowSpacing": 10,
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
