var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#ProductListTable').DataTable({
        "ajax": { "url":"/admin/product/GetPaggedList"},
        columns: [
            { "data": "title" },
            { "data": "serialNo" },
            { "data": "price" },
            { "data":"category.name"},
            { "data": "manufacturer" }
        ]
    });
}