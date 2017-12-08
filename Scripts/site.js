function Action(actionName, controllerName, parameters) {
    console.log('action: ' + actionName);
    console.log('controller: ' + controllerName);
    console.log('parameter: ' + parameters);



    if (!isNaN(parseInt(parameters))) {
        alert(1);
        parameters = "/" + parameters;
    } else if (typeof parameters === "object") {
        alert(2);
        try {
            var p = "";
            for (var key in parameters) {
                p += "&" + key + "=" + parameters[key];
            }
            parameters = "?" + p.substring(1, p.length);
        } catch (e) {
            parameters = "";
        }
    } else if (parameters != undefined && parameters != null && parameters != "") {
        parameters = "?" + parameters;
    } else {
        parameters = "";
    }

    var r = rootUrl + controllerName + "/" + actionName + parameters;
    console.log(r);

    return r;
}


