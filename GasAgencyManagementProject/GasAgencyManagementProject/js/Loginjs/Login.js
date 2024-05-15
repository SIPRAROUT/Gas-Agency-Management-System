$(document).ready(function () {
    debugger;
    hideLoader();
    $('#txt_email').focus();

    $('#txt_email, #txt_password').on('keyup', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            Loginchk();
        }
    });
    $('#txt_password').attr("type", "password");
});
function togglePasswordVisibility() {
    var passwordField = $("#txt_password");
    var toggleButton = $(".toggle-password i");

    if (passwordField.attr("type") === "password") {
        passwordField.attr("type", "text");
        toggleButton.removeClass("fa-eye").addClass("fa-eye-slash");
    } else {
        passwordField.attr("type", "password");
        toggleButton.removeClass("fa-eye-slash").addClass("fa-eye");
    }
}
$('.toggle-password').click(function () {
    togglePasswordVisibility();
});
document.addEventListener('keydown', function (event) {
    // Check if the pressed key is Escape (key code 27)
    if (event.key === 'Escape') {
        $('#txt_email').val('');
        $('#txt_password').val('');

    }
});
function showLoader() {
    // Show the loader
    $('#loader').show();
}
function hideLoader() {
    // Hide the loader
    $('#loader').hide();
}
function Loginchk() {
    debugger;
    if (ValidateLogin() == true) {
        debugger;
        $('#txt_email').focus();

        $.ajax({
            type: 'post',
            dataType: 'JSON',
            url: '@Url.Action("Loginchk")',
            data: { 'useremail': document.getElementById('txt_email').value, 'pwd': document.getElementById('txt_password').value },
            success: function (res) {
                debugger;
                if (res.toString().split('|')[0].trim() == "#sucess") {
                    showLoader();// Show loader before AJAX reques
                    setTimeout(function () {
                        window.location.href = "../Master/DashBoard";
                    }, 2000);

                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops',
                        text: 'Invalid Useremail/Password'
                    });
                    ClearUser();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
}
//-----------------------------
function ClearUser() {
    $('#txt_email').val('');
    $('#txt_password').val('');
}
//-----------------------------
function ValidateLogin() {

    if ($('#txt_email').val() === "") {
        $("#txt_email").focus();

        Swal.fire({
            icon: 'info',
            text: 'Email cannot be  blank!!'
        });
        return false;
    }
    if ($('#txt_password').val() === "") {
        $("#txt_password").focus();
        Swal.fire({
            icon: 'info',
            text: 'Password cannot be blank!!'
        });
        return false;
    }
    return true;
}