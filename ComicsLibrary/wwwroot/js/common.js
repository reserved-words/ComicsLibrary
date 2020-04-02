
AJAX = {
    get: function (url, onLoaded) {
        index.loading(true);

        //mgr.getUser().then(function (user) {
        //    console.log(user.access_token);
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            onLoaded(JSON.parse(xhr.responseText));
            index.loading(false);
        }
        //    xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
        //});
    },

    post: function (url, data, onLoaded) {
        index.loading(true);

        //mgr.getUser().then(function (user) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url);
        xhr.onload = function () {
            //alert(xhr.responseText);
            //onLoaded(JSON.parse(xhr.responseText));


            onLoaded(xhr.responseText ? JSON.parse(xhr.responseText) : '');
            index.loading(false);
        };
        //  xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send(data);
        //   });
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