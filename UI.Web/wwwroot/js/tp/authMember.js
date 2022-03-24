function verify_createLogin() {
    if ($('#IsCreateLogin').prop('checked')) {
        if ($('#Password').val() == '') {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please enter Password.!',
            });
            return false;
        }
    }
    return true;
}

$(document).ready(function () {
    $('#IsCreateLogin').on('change', function () {
        if ($('#IsCreateLogin').prop('checked')) {
            $('#Password').removeAttr('disabled');
            $('#ConfirmPassword').removeAttr('disabled');
        }
        else {
            $('#Password').attr('disabled', '');
            $('#ConfirmPassword').attr('disabled', '');
        }
    });

    $('#IsCreateLogin').change();
});