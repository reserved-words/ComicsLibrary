
API = {
    get: function (url, onLoaded) {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url, true);
        authorizeAndSend(xhr, null, onLoaded);
    },
    post: function (url, data, onLoaded) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.setRequestHeader("Accept", "application/json");
        authorizeAndSend(xhr, JSON.stringify(data), onLoaded);
    }
};

function authorizeAndSend(xhr, data, onLoaded) {
    xhr.onreadystatechange = function (oEvent) {
        onReadyStateChange(xhr, onLoaded);
    };

    mgr.getUser().then(function (user) {
        if (user) {
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send(data);
        }
        else {
            mgr.signinRedirect();
        }
    });
}

function onReadyStateChange(xhr, onLoaded) {
    if (xhr.readyState !== 4)
        return;

    if (xhr.status === 401) {
        alert("Your session has expired");
        mgr.signinRedirect();
        // Ideally want the callback to then retry the request
        return;
    }

    if (xhr.status === 200 || xhr.status === 204) {
        if (onLoaded) {
            onLoaded(xhr.responseText ? JSON.parse(xhr.responseText) : '');
        }
    } else {
        alert("Error: " + xhr.statusText);
        console.error(xhr.statusText);
    }
}