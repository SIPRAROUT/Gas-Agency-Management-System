$(document).ready(function () {
    $.noConflict();
    debugger;
   
    // Trigger click event on the "VIEW" tab if read access is enabled
    if (canReadAccess && !canWriteAccess) {
        $('#tab2').click();
    }
    $('#tab1').click(function () {
        ClearArea();
    });
    $('#txt_AreaName').focus();
    $('form').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault(); // Prevent the default Enter key behavior (e.g., submitting a form)
            if ($('#btn_cancel').is(':focus')) {
                e.stopPropagation();
                ClearArea(); // Call ClearSupplier if the clear button is focused
            }
            else {
                SupplierIUDOnEnter(); // Call SupplierIUDOnEnter for form submission
            }
        }
    });
});
// Custom function to handle form submission on Enter key
function SupplierIUDOnEnter() {
    AreaIUD();
}
document.addEventListener('keydown', function (event) {
    // Check if the pressed key is Escape (key code 27)
    if (event.key === 'Escape') {
        ClearArea();
    }
});
//UserValidation ALertMsg
function AreaIUD() {
    var ButtonData = $('#btn_submit');
    var action = "";
    if (ButtonData.text() === "SUBMIT") {
        debugger;
        action = "INSERT"
    }
    else {
        debugger;
        action = "UPDATE"
    }
    $('#txt_AreaName').focus();
    if (ValidateUser() == true) {
        $.ajax({
            type: 'post',
            dataType: 'JSON',
            url: '../Master/MstrAreaIUD',
            data: {
                'Area_Id': $('#hdnId').val(),
                'Area_Info': $('#txt_AreaName').val(), 'Area_Details': $('#Details').val(),
                'Action': action
            },
            success: function (res) {
                debugger;
                if (res == 'Area Master Created Successfully') {
                    $('#txt_AreaName').focus();
                    swal.fire({
                        icon: 'success',
                        title: "Success",
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
                    });
                    ClearArea();
                }
                else if (res == 'Area Master Modified Successfully') {
                    $('#txt_AreaName').focus();
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
                    });
                    ClearArea();
                    $('#btn_submit').text('SUBMIT');
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops',
                        text: res
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
}
//UserValidation PopupMsg
function ValidateUser() {
    if ($('#txt_AreaName').val() === "") {
        $('#txt_AreaName').focus();
        Swal.fire({
            icon: 'info',
            title: 'Info',
            text: 'Area Name cannot be  blank!!'
        });
        return false;
    }
    if ($('#Details').val() === "") {
        $('#Details').focus();
        Swal.fire({
            icon: 'info',
            title: 'Info',
            text: 'Area Details cannot be blank!!'
        });
        return false;
    }
    return true;
}
//ClearArea
function ClearArea() {
    $('#txt_AreaName').val('');
    $('#Details').val('');

    $('#btn_submit').innerText = 'Submit';
    $('#txt_AreaName').focus();
    $("#btn_submit").text("SUBMIT")
}
//-----------------------------------------------------view page------------------------------------------//
setTimeout(function () {
    initializeDataTable();
}, 0);
var initializeDataTable = function () {
    if ($.fn.DataTable.isDataTable('#tblViewArea')) {
        $('#tblViewArea').DataTable().destroy();
    }
    var dt = $('#tblViewArea').DataTable(
        {
            "processing": true,
            "responsive": true,
            "serverSide": true,
            "searching": true,
            "paging": true,
            "sort": true,
            /*  "ajax": "/Master/GetConsumer",*/
            "ajax": {
                "url": "/Master/GetArea",
                "type": "GET"  // Assuming a POST request, change it to "GET" if needed
            },
            "columns": [
                {
                    "className": "text-center",
                    "data": null,
                    "orderable": false,
                    "render": function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    },
                },
                { "data": "Area_Info" },
                { "data": "Area_Details" },
                {
                    "class": "details-control",
                    "orderable": false,
                    "data": null, render: function (data, type, row) {
                        return "<div class='btn-group' style='text-align:center;' ><a  id='" + data.Area_Id + "' onclick='return_id(this.id);' style='cursor:pointer;' title='Edit' class='btn btn-primary action_btn'><i class='jsgrid-button jsgrid-edit-button ti-pencil-alt btn btn-primary color-white'></i></a>" +
                            "<a id='" + data.Area_Id + "' onclick='delete_id(this.id);' style='cursor:pointer;' title='Delete' class='btn btn-danger action_btn'><i class='jsgrid-button jsgrid-delete-button ti-trash btn btn-danger color-white'></i></a></div>";
                    }
                }
            ],
            "order": [[0, 'asc']]
        });
}
function toggleTabs(tabId) {
    if (tabId === 'content1') {
        debugger;
        $('#content1').show();
        $('#txt_AreaName').focus();
        $('#content2').hide();
        debugger;
    }
    else if (tabId === 'content2') {
        setTimeout(function () {
            if ($("#btn_submit").text() == "UPDATE") {
                ClearArea();
                $("#btn_submit").text("SUBMIT")
            }
            initializeDataTable();
            $('#content1').hide();
            $('#content2').show();
        }, 0);
        debugger;
    }
}

//----------edit data---------------
function return_id(par) {
    var canEdit = @Html.Raw(Json.Encode(canWrite1));
    if (!canEdit) {
        Swal.fire({
            icon: 'error',
            title: 'Oops',
            text: 'YOU HAVE NO PERMISSION FOR EDIT!'
        });
        return;
    }
    $('#tab1').addClass('active');
    $('#tab2').removeClass('active').addClass('closed');
    $('#content1').show();
    $('#content2').hide();
    debugger;
    $.ajax(
        {
            url: '/Master/EditArea?Id=' + par, // Replace with your controller and action
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#hdnId').val(par),
                    $('#txt_AreaName').focus(),
                    $('#txt_AreaName').val(data.Area_Info)
                $('#Details').val(data.Area_Details)
                $('#btn_submit').text('UPDATE');
            },
            error: function () {
                console.log('Error loading data.');
            }
        });
}
//-----------end here---------------
//-----------delete data--------------
function delete_id(par) {
    debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            var equip = {
                'Area_Id': par,
                'Action': 'DELETE'
            };
            $.ajax({
                url: '/Master/MstrAreaIUD',
                data: JSON.stringify(equip),
                type: 'post',
                dataType: 'json',
                contentType: 'application/json',
                success: function (res) {
                    if (res === "Area Master Deleted Successfully") {
                        Swal.fire({
                            icon: 'success',
                            title: "Deleted",
                            text: res
                        });
                        setTimeout(function () {
                            debugger;
                            /*  window.location.href = "/Master/Consumer";*/
                            initializeDataTable();
                            debugger;
                            $('#content1').hide();
                            $('#content2').show();
                        }, 100);
                    }
                    else if (res === "YOU HAVE NO PERMISSION FOR DELETE!") {
                        Swal.fire({
                            icon: 'error',
                            title: "Opss",
                            text: res
                        });
                    }
                    else if (res == 'Area is referenced in the another table. Cannot delete.') {
                        Swal.fire({
                            icon: 'error',
                            title: "Oops",
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
                    else {
                        alert(res);
                    }
                },
                error: function (err) {
                    alert(err);
                }
            });
        }
    });
}