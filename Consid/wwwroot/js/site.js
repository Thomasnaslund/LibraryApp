// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});


//Input helper for Create View on Libraryitems
typeInputSwitch = val => {
    $("#category").show();
    $("#title").show();
    $("#submit").show();
    if (val == "Book") {
        $("#rtm").hide();
        $("#author").show();   
        $("#pages").show();
        $("#author").show();
        $("#isbarrowable").show();
    } else if (val == "Reference book") {
        $("#rtm").hide();
        $("#pages").show();
        $("#author").show();
        $("#isbarrowable").hide();
    } else {
        $("#pages").hide();
        $("#author").hide();
        $("#rtm").show();
        $("#isbarrowable").show();
    }
}

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}

refreshFormAjax = (url, title) => {
    var $frm = $('#form1');
    console.log($frm);
    $.ajax({
        type: 'POST',
        url: url,
        data: $frm.serialize(),
        success: function (res) {
            $('#form-modal .modal-body').html(res.html);
            $('#form-modal .modal-title').html(title);
        }
    })
}


jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else {
                    alert(res.errormsg);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $('#view-all').html(res.html);
                    }
                    else {
                        alert(res.errormsg);
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $('#view-all').html(res.html);
                    }
                    else {
                        alert(res.errormsg);
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}