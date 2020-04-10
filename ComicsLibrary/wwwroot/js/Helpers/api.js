
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
    index.loading(true);

    xhr.onreadystatechange = function (oEvent) {
        onReadyStateChange(xhr, onLoaded);
    };

    mgr.getUser().then(function (user) {
        if (user) {
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send(data);
        }
        else {
            mgr.signinRedirect()
        }
    });
}

function onReadyStateChange(xhr, onLoaded) {
    if (xhr.readyState !== 4)
        return;

    if (xhr.status === 200) {
        index.loading(false);
        onLoaded(xhr.responseText ? JSON.parse(xhr.responseText) : '');
    } else {
        alert("Error");
        console.error(xhr.statusText);
    }
}