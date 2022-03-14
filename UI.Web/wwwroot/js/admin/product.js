var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#ProductListTable').DataTable({
        //"processing": true,
        //"serverSide": true,
        "ajax": { "url":"/admin/product/GetPaggedList"},
        columns: [
            { "data": "title" },
            { "data": "serialNo" },
            { "data": "price" },
            { "data":"category.name"},
            { "data": "manufacturer" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="btn-group" role="group">
                        <a class="btn btn-outline-primary" href="/Admin/Product/Upsert?id=${data}"> <i class="bi bi-pencil-square">&nbsp;</i> Edit</a>
                        <a class="btn btn-outline-danger" OnClick=Delete('/Admin/Product/Delete/+${data}')> <i class="bi bi-trash">&nbsp;</i> Delete</a>
                    </div>`;
                }
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure ?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}