// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function loadPartialView(action, data, callback) {
    $.ajax({
        type: "GET",
        url: action,
        contentType: "application/json; charset=utf-8",
        data: data,
        datatype: "json",
        success: callback,
        error: function () {
            toastr.error("Failed to laod contents.");
        }
    });
}


function PopulateDropDown(dropDownId, list) {
    $(dropDownId).empty();
    $.each(list, function (index, row) {
        if (index == 0) {
            $(dropDownId).append("<option value='" + row.value + "' selected='selected'>" + row.text + "</option>");
        } else {
            $(dropDownId).append("<option value='" + row.value + "'>" + row.text + "</option>")
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure want to delete ?',
        text: "You won't be able to revert this action!",
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
                        location.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                error: function (error,xhr)
                {
                    toastr.error('Error:- Invalid End point.');
                }
              
            });
        }
    })
}