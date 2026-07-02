function getTimeUI() {
    var currentdate = new Date();

    var day = currentdate.getDate();
    if (day < 10) day = "0" + day;

    var month = currentdate.getMonth() + 1;
    if (month < 10) month = "0" + month;

    var hours = currentdate.getHours();
    if (hours < 10) hours = "0" + hours;

    var minutes = currentdate.getMinutes();
    if (minutes < 10) minutes = "0" + minutes;

    var seconds = currentdate.getSeconds();
    if (seconds < 10) seconds = "0" + seconds;

    return datetime = day + "/"
        + month + "/"
        + currentdate.getFullYear() + " "
        + hours + ":"
        + minutes + ":"
        + seconds;
}

function getTime() {
    var currentdate = new Date();

    var day = currentdate.getUTCDate();
    if (day < 10) day = "0" + day;

    var month = currentdate.getUTCMonth() + 1;
    if (month < 10) month = "0" + month;

    var hours = currentdate.getUTCHours();
    if (hours < 10) hours = "0" + hours;

    var minutes = currentdate.getUTCMinutes();
    if (minutes < 10) minutes = "0" + minutes;

    var seconds = currentdate.getUTCSeconds();
    if (seconds < 10) seconds = "0" + seconds;

    return parseInt(datetime = currentdate.getUTCFullYear() + ""
        + month + ""
        + day + ""
        + hours + ""
        + minutes + ""
        + seconds);
}

var statusEle = null;
var updatedEle = null;
var iteration = 0;
var interval = 5;
var startDatetime = getTime();

var basePath = window.location.pathname.includes("/view-latest-funding") ? "/view-latest-funding" : "/single-funding-statement/latest";

var url = basePath + "/admin/publication/getspreadsheetcreateddate/" + fundingStreamCode + "/" + fundingPeriodCode + "/" + publishedDate + "/" + startDatetime;

var sInt = setInterval(function () {
    var datetime = getTimeUI();

    if (!updatedEle) updatedEle = document.getElementById("updated");
    updatedEle.innerHTML = datetime;

    if (!statusEle) statusEle = document.getElementById("status");

    iteration += interval;

    if (iteration > 300) {
        statusEle.innerHTML = "Failed - Error";
        clearInterval(sInt);
    }
    else {
        $.get(url, function (data) {
            var serverDatetime = parseInt(data);

            if (serverDatetime > startDatetime) {
                statusEle.innerHTML = "Generated successfully";
                clearInterval(sInt);
            }
            else {
                statusEle.innerHTML += ".";
            }
        });
    }

}, 1000 * interval);