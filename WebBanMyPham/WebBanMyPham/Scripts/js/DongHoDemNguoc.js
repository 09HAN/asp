var mins
var secs;

function DongHoNguoc(phut, giay, id) {
    mins = 1 * m(phut);
    secs = 0 + s(giay);
    redo(id);
}

function m(obj) {
    for (var i = 0; i < obj.length; i++) {
        if (obj.substring(i, i + 1) == ":")
            break;
    }
    return (obj.substring(0, i));
}

function s(obj) {
    for (var i = 0; i < obj.length; i++) {
        if (obj.substring(i, i + 1) == ":")
            break;
    }
    return (obj.substring(i + 1, obj.length));
}

function dis(mins, secs) {
    var disp;
    if (mins <= 9) {
        disp = " 0";
    } else {
        disp = " ";
    }
    disp += mins + ":";
    if (secs <= 9) {
        disp += "0" + secs;
    } else {
        disp += secs;
    }
    return (disp);
}

function redo(id) {
    secs--;
    if (secs == -1) {
        secs = 59;
        mins--;
    }
    document.getElementById(id).innerHTML = dis(mins, secs);
    if ((mins == 0) && (secs == 0)) {
    }
    else {
        DongHoNguoc = setTimeout("redo('" + id + "')", 1000);
    }

}