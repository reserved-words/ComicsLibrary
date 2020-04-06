
AJAX = {
    get: function (url, onLoaded) {
        index.loading(true);

        mgr.getUser().then(function (user) {
            var xhr = new XMLHttpRequest();
            xhr.open("GET", url, true);
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);

            xhr.onreadystatechange = function (oEvent) {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        onLoaded(JSON.parse(xhr.responseText));
                        index.loading(false);
                    } else {
                        alert("Error");
                        console.log("Error", xhr.statusText);
                    }
                }
            };
            xhr.send();
        });
    },

    post: function (url, data, onLoaded) {
        index.loading(true);

        mgr.getUser().then(function (user) {
            var xhr = new XMLHttpRequest();
            xhr.open("POST", url);
            xhr.onload = function () {
                onLoaded(xhr.responseText ? JSON.parse(xhr.responseText) : '');
                index.loading(false);
            };
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.setRequestHeader("Accept", "application/json");
            xhr.send(JSON.stringify(data));
        });
    },

    getContent: function (path, onFetched) {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", path, false);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                if (xhr.status === 200 || xhr.status == 0) {
                    var content = JSON.parse(xhr.responseText);
                    onFetched(content);
                }
            }
        }
        xhr.send(null);
    }
};

requirejs.config({
    baseUrl: URL.base + '/lib',
    waitSeconds: 200,
    paths: {
        'knockout': 'KnockoutJS/knockout-3.5.0',
        text: 'RequireJS/text'
    }
});

ko.components.register('comic', {
    viewModel: { require: URL.base + '/js/Components/comic.js' },
    template: { require: 'text!../js/Templates/comic.html' }
});

ko.components.register('issue', {
    viewModel: { require: URL.base + '/js/Components/issue.js' },
    template: { require: 'text!../js/Templates/issue.html' }
});

ko.components.register('result', {
    viewModel: { require: URL.base + '/js/Components/result.js' },
    template: { require: 'text!../js/Templates/result.html' }
});