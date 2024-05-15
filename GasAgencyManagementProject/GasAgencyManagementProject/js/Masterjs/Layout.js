function openChangePasswordModal() {
    openModal();
    initializePasswordChangeModal();
    $('#txtCurrentPassword').attr("type", "password");
    $('#txtNewPassword').attr("type", "password");
    $('#txtConfirmPassword').attr("type", "password");
}

// Function to open the modal
function openModal() {
    document.getElementById('myModal').style.display = 'block';

}
function initializePasswordChangeModal() {
    // ... (your existing code for the modal)

    // Move the focus and keypress event handling to a separate function
    function handleKeyPressOnModal(e) {
        if (e.which === 13) {
            e.preventDefault();
            validate1();
        }
    }
    $('#txtCurrentPassword').focus();

    // Add keypress event handling for the modal
    $('form').off('keypress');
    $('form').keypress(handleKeyPressOnModal);
}
function toggleCurrentPasswordVisibility() {
    var passwordField = $("#txtCurrentPassword");
    var toggleButton = $(".toggle-password i");

    if (passwordField.attr("type") === "password") {
        passwordField.attr("type", "text");
        toggleButton.removeClass("fa-eye").addClass("fa-eye-slash");

    } else {
        passwordField.attr("type", "password");
        toggleButton.removeClass("fa-eye-slash").addClass("fa-eye");

    }
}
function toggleNewPasswordVisibility() {
    var passwordField = $("#txtNewPassword");
    var toggleButton = $(".toggle-password i");

    if (passwordField.attr("type") === "password") {
        passwordField.attr("type", "text");
        toggleButton.removeClass("fa-eye").addClass("fa-eye-slash");

    } else {
        passwordField.attr("type", "password");
        toggleButton.removeClass("fa-eye-slash").addClass("fa-eye");

    }
}
function ConfirmPasswordVisibility() {
    var passwordField = $("#txtConfirmPassword");
    var toggleButton = $(".toggle-password i");

    if (passwordField.attr("type") === "password") {
        passwordField.attr("type", "text");
        toggleButton.removeClass("fa-eye").addClass("fa-eye-slash");

    } else {
        passwordField.attr("type", "password");
        toggleButton.removeClass("fa-eye-slash").addClass("fa-eye");

    }
}
document.addEventListener('keydown', function (event) {
    // Check if the pressed key is Escape (key code 27)
    if (event.key === 'Escape') {
        Clear();
    }
});
// Function to close the modal
function closeModal() {
    document.getElementById('myModal').style.display = 'none';
    $('form').off('keypress');
}




var txtCurrentPassword = $('#txtCurrentPassword');
var txtNewPassword = $('#txtNewPassword');
var txtConfirmPassword = $('#txtConfirmPassword');
//------------------------------------------------------

function validate1() {
    $('#txtCurrentPassword').focus();
    if (ValidateChangePassword() == true) {

        //-----------------------------------------------------------------
        $.ajax({
            type: 'post',
            dataType: 'JSON',
            url: '../Master/SavePass',
            data: { 'Cpass': txtCurrentPassword.val(), 'Npass': txtNewPassword.val(), 'CFpass': txtConfirmPassword.val() },
            success: function (res) {

                debugger;
                if (res === "Problem Accessing Database. Please Try Again") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops',
                        text: res,
                        didOpen: () => {
                            // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                            setTimeout(() => {
                                const okButton = document.querySelector('.swal2-confirm');
                                if (okButton) {
                                    okButton.focus();
                                }
                            }, 0);
                        },
                    })
                    Clear();
                    txtCurrentPassword.focus();
                }
                else if (res === "Password Changed  Successfully") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Updated',
                        text: res,
                        didOpen: () => {
                            // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                            setTimeout(() => {
                                const okButton = document.querySelector('.swal2-confirm');
                                if (okButton) {
                                    okButton.focus();
                                }
                            }, 0);
                        },
                    })
                    Clear();
                    $('#txtCurrentPassword').focus();

                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops',
                        text: res,
                        didOpen: () => {
                            // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                            setTimeout(() => {
                                const okButton = document.querySelector('.swal2-confirm');
                                if (okButton) {
                                    okButton.focus();
                                }
                            }, 0);
                        },
                    })
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
                alert("An error occurred during the AJAX request. Please try again.");
            }
        });


    }
}

//--------------------
function ValidateChangePassword() {
    //var Errormsg = new Array();
    debugger;
    if ($('#txtCurrentPassword').val() == "") {
        $('#txtCurrentPassword').focus();
        Swal.fire(
            {
                icon: 'info',
                title: 'Info',
                text: 'Current Password can not be left blank!!',
                didOpen: () => {
                    // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                    setTimeout(() => {
                        const okButton = document.querySelector('.swal2-confirm');
                        if (okButton) {
                            okButton.focus();
                        }
                    }, 0);
                },
            }
        );
        return false;
        //Errormsg.push('Current Password can not be left blank');
    }
    if ($('#txtNewPassword').val() == "") {
        $('#txtNewPassword').focus();
        Swal.fire(
            {
                icon: 'info',
                title: 'Info',
                text: 'New Password can not be left blank!!',
                didOpen: () => {
                    // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                    setTimeout(() => {
                        const okButton = document.querySelector('.swal2-confirm');
                        if (okButton) {
                            okButton.focus();
                        }
                    }, 0);
                },
            }

        );
        $('#txtNewPassword').focus();
        return false;
    }
    if ($('#txtConfirmPassword').val() == "") {
        $('#txtConfirmPassword').focus();
        Swal.fire(
            {
                icon: 'info',
                title: 'Info',
                text: 'Confirm Password can not be left blank!!',
                didOpen: () => {
                    // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                    setTimeout(() => {
                        const okButton = document.querySelector('.swal2-confirm');
                        if (okButton) {
                            okButton.focus();
                        }
                    }, 0);
                },
            }
        );
        return false;

    }
    if ($('#txtCurrentPassword').val() != "" && $('#txtNewPassword').val() != "") {
        if ($('#txtCurrentPassword').val() == $('#txtNewPassword').val()) {
            $('#txtNewPassword').focus();
            Swal.fire(
                {
                    icon: 'info',
                    title: 'Info',
                    text: 'New password must be different from current passwords used!!',
                    didOpen: () => {
                        // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                        setTimeout(() => {
                            const okButton = document.querySelector('.swal2-confirm');
                            if (okButton) {
                                okButton.focus();
                            }
                        }, 0);
                    },
                }

            );
            return false;
        }
    }
    if ($('#txtNewPassword').val() != $('#txtConfirmPassword').val()) {
        $('#txtConfirmPassword').focus();
        Swal.fire(
            {
                icon: 'info',
                title: 'Info',
                text: 'New Password & Confirm Password Should be Same!!',
                didOpen: () => {
                    // Use a timeout to ensure the Swal modal is rendered before attempting to set focus
                    setTimeout(() => {
                        const okButton = document.querySelector('.swal2-confirm');
                        if (okButton) {
                            okButton.focus();
                        }
                    }, 0);
                },
            }

        );
        return false;
        //Errormsg.push('New Password & Confirm Password Should be Same');
    }


    return true;
}
//-------------------------
function Clear() {
    $('#txtCurrentPassword').val('');
    $('#txtNewPassword').val('');
    $('#txtConfirmPassword').val('');
    $('#txtCurrentPassword').focus();
}


//logout


function Logout() {
    $.ajax({
        type: 'get',
        url: '/Login/Logout',
        success: function () {
            // Clear browser history
            window.location.replace('/Login/Login');
            window.history.replaceState({}, document.title, window.location.pathname);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            // Handle errors if needed
            console.error("Error during logout:", thrownError);
        }
    });
}

// lock screen
const lockScreenButton = document.getElementById('open-lock-screen');
const lockScreen = document.getElementById('lock-screen');

lockScreenButton.addEventListener('click', () => {
    lockScreen.classList.add("lock-screen-active");
});

// Capture clicks on the entire lock screen element
lockScreen.addEventListener('click', () => {
    // Redirect to the previous page when the lock screen is clicked
    lockScreen.classList.remove("lock-screen-active");
});

document.addEventListener('keydown', (event) => {
    // Check if the pressed key is Enter (key code 13) or Spacebar (key code 32)
    if (event.key === 'Enter' || event.key === ' ') {
        lockScreen.classList.remove("lock-screen-active");
    }
});