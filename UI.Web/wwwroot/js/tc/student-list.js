var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    var status = 'all';
    if (url.includes("pending")) {
        status = 'pending';
    }
    else if (url.includes("completed")) {
        status = 'completed';
    }
    else if (url.includes("inprocess")) {
        status = 'inprocess';
    }
    else if (url.includes("cancelled")) {
        status = 'cancelled';
    }
    else if (url.includes("approved")) {
        status = 'approved';
    }
    var filter = {
        orderStatus: status
    }
    loadDataTable(filter);

});

function loadDataTable(filter) {

    dataTable = $('#studentListTable').DataTable({
        //"processing": true,
        //"serverSide": true,
        "ajax": { "url": "/TrainingCenter/Student/StudentListPage", "data": filter },

        columns: [
            {
                "title": "Serial",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "id" },
            {
                data: null,
                render: function (data, type, row) {
                    // Combine the first and last names into a single table field
                    return data.firstName + ' ' + data.lastName;
                },
                editField: ['firstName', 'lastName']
            },

            {
                "data": "dob",
               /* render:  DataTable.render.moment('MMM DD, YY')*/
            },
            { "data": "phoneNumber" },
            { "data": "email" },
            { "data": "course.courseName" },
            { "data": "createdOn" },
            { "data": "applicationUser.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="btn-group text-end" role="group">
                           <a class="btn btn-outline-primary" href="/Admin/Order/Detail?orderId=${data}"> <i class="bi bi-list-ul">&nbsp;</i> Details</a>
                       
                    </div>`;/* <a class="btn btn-outline-primary" href="#" onclick="showOrderDetail('/Admin/Order/_Detail?orderId=${data}')" data-bs-toggle="modal" data-bs-target="#orderDetailModal"> <i class="bi bi-list-ul">&nbsp;</i> Details</a>*/
                }
            }
        ]
    });
}
