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
    dataTable = $('#OrderListTable').DataTable({
        //"processing": true,
        //"serverSide": true,
        "ajax": { "url": "/admin/Order/GetPaggedList", "data": filter },
        columns: [
            { "data": "id" },
            { "data": "name" },
            { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderTotal" },
            { "data": "orderStatus" },
            { "data": "paymentStatus" },
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

function showOrderDetail(action) {
    loadPartialView(action, null, function (response) {
        $('#orderDetailModalContent').html(response);
    });
}


