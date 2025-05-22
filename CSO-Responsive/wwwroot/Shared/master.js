function showLoader($obj) {
    $obj.LoadingOverlay("show", { color: "rgba(0, 0, 0, 0.4)" });
}

function hideLoader($obj) {
    $obj.LoadingOverlay("hide", true);
}

//function showAlert(alert) {
//    if (!alert.title) {
//        swal({
//            position: 'top',
//            type: alert.alertType.toLowerCase(),
//            title: alert.message,
//            showConfirmButton: true,
//            toast: true
//        });
//    } else {
//        swal({
//            position: 'top',
//            type: alert.alertType.toLowerCase(),
//            title: alert.title,
//            text: alert.message,
//            showConfirmButton: true,
//            toast: true
//        });
//    }
//};


//function showAlertLong(alert) {

//    if (!alert.Title) {
//        window.swal({
//            position: 'top-middle',
//            type: alert.AlertType.toLowerCase(),
//            title: alert.Message,
//            showConfirmButton: false,
//            timer: 20000,
//            toast: true
//        });
//    } else {
//        window.swal({
//            position: 'top-middle',
//            type: alert.AlertType.toLowerCase(),
//            title: alert.Title,
//            text: alert.Message,
//            showConfirmButton: false,
//            timer: 20000,
//            toast: true
//        });
//    }
//};

//function showErrorAlert() {

//    var errorAlert = {
//        AlertType: 'Error',
//        Message: "Error occurred."
//    };
//    showAlert(errorAlert);
//}

//function showErrorAlertWithButton(title, msg) {
//    sweetAlert(
//        title,
//        msg,
//        'error'
//    )
//}

//function showSuccessAlert() {
//    var successAlert = {
//        AlertType: 'Success',
//        Message: "Operation completed successfully."
//    };
//    showAlert(successAlert);
//}

function convertToEmpty(value) {
    if (value === undefined || value === null || value === "null")
        return '';
    else
        return value;
}

function convertUTCToLocalDateTime(utcDt, utcDtFormat) {
    var toDt = moment.utc(utcDt, utcDtFormat).toDate();
    return moment(toDt).format('YYYY-MM-DD hh:mm:ss A');
}

function convertUTCToLocalTime(utcDt, utcDtFormat) {
    if (utcDt && utcDt != "0001-01-01 00:00:00" && utcDt != null) {
        var toDt = moment.utc(utcDt, utcDtFormat).toDate();
        return moment(toDt).format('HH:mm');
    } else {
        return "00:00:00"
    }
}

function getCurrentTimeZone() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function getUrlParam(key) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(key);
}

function getStartDate() {
    return $('#FilterDate').data('daterangepicker').startDate.toISOString();
    //var sDate = $('#FilterDate').data('daterangepicker').startDate.toISOString();
    //sDate = sDate.split("T")[0] + "T23:59:59.999Z";
    //return sDate;
}

function getEndDate() {
    return $('#FilterDate').data('daterangepicker').endDate.toISOString();
    //var eDate = $('#FilterDate').data('daterangepicker').endDate.toISOString();
    //eDate = eDate.split("T")[0] + "T23:59:59.999Z";
    //return eDate;
}

function getUserName() {
    return $('#SearchUser').val();
}

function reportDefaultParam(username = null) {
    return 'username=' + (username && username != "" ? username : getUserName()) + '&startDate=' + getStartDate() + '&endDate=' + getEndDate();
}

function convertSecondToTime(s) {
    var fm = [
        Math.floor(s / 60 / 60) % 24, // HOURS
        Math.floor(s / 60) % 60, // MINUTES
        //s % 60 // SECONDS
    ];
    return $.map(fm, function (v, i) { return ((v < 10) ? '0' : '') + v; }).join(':');
}

function displayTime(ticksInSecs) {
    var ticks = ticksInSecs;
    var hh = Math.floor(ticks / 3600);
    var mm = Math.floor((ticks % 3600) / 60);
    var ss = ticks % 60;

    return pad(hh, 2) + ":" + pad(mm, 2) + ":" + pad(ss, 2);
}

function pad(n, width) {
    var n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join('0') + n;
}

// Get the container element
var Container = document.getElementById("sidemenu");
if (Container != null)
    // Get all li with class="nav-link" inside the container
    var navlink = Container.getElementsByClassName("nav-link");
if (navlink != null)
    // Loop through the a and add the active class to the current/clicked a
    for (var i = 0; i < navlink.length; i++) {
        navlink[i].addEventListener("click", function () {
            var current = document.getElementsByClassName("active");
            current[0].className = current[0].className.replace(" active", "");
            this.className += " active";
        });
    }


$(function () {
    $('body').toggleClass('sidebar-xs').removeClass('sidebar-mobile-main');
    LoadFinancialYear();
    $("#iProfileIcon").click(function () {
        $("#myModal_Profile").modal({ backdrop: 'static', keyboard: false });
        //$.ajax({
        //    type: 'post',
        //    url: "/Home/GetUserData",
        //    datatype: 'json',
        //    success: function (response) {
        //        debugger;
        //        try {

        //            var items = response.aItemList;
        //            $('#txtADid').val(items["aDid"]);
        //            $('#txtName').val(items["name"]);
        //            $('#txtEmail').val(items["email"]);
        //            $('#txtDesignation').val(items["designation"]);
        //            $('#txtMobile').val(items["mobileNo"]);
                    
        //        } catch (e) {

        //        }
        //        $('#LoadingImage').hide();
        //    },
        //    error: function (result) {
        //        alert(result);
        //        $('#LoadingImage').hide();
        //    }
        //});

       
    });
});

//Load Year

function LoadFinancialYear() {
    const d = new Date();
    var year = d.getFullYear();  // returns the current year
    //debugger;

    //$("[id*= ddlFinancialYear]").empty();

    //for (i = year; i >= 2021; i--) {
    //    let nexyear = i + 1;

    //    $("[id*= ddlFinancialYear]").append("<option value='" + i.toString().substring(2) + nexyear.toString().substring(2) + "'>" + i.toString() + '-' + nexyear.toString().substring(2) + "</option>");
    //}
    //if ((d.getMonth() + 1) > 3) {
    //    let nexyear = year + 1;
    //    $("[id*= ddlFinancialYear]").val(year.toString().substring(2) + nexyear.toString().substring(2));
    //}
    //else {
    //    let prev = year - 1;
    //    $("[id*= ddlFinancialYear]").val(prev.toString().substring(2) + year.toString().substring(2));
    //}
}

