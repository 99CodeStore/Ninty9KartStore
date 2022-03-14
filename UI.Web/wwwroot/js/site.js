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