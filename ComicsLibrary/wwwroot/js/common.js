URL = {
    get: function (name, id, offset, page, sortOrder, title) {
        var actionUrl = $('#' + name + 'Url').attr('data-stuff-url');
        return actionUrl
            .replace('=id', '=' + id)
            .replace('=Id', '=' + id)
            .replace('=offset', '=' + offset)
            .replace('=page', '=' + page)
            .replace('=sortOrder', '=' + sortOrder)
            .replace('=title', '=' + title);
    },

    base: window.location.href
};

AJAX = {
    get: function (url, onLoaded) {
        index.loading(true);
        $.ajax({
            method: "GET",
            url: url,
            success: function (data) {
                onLoaded(data);
                index.loading(false);
            },
            error: function (ex) {
                alert("Error");
            }
        });
    },
    post: function (url, data, onLoaded) {
        index.loading(true);
        $.ajax({
            url: url,
            method: "POST",
            data: data
        })
        .done(function (result) {
            onLoaded(result);
        })
        .fail(function (jqXHR, textStatus) {
            alert("Request failed: " + textStatus);
            alert(JSON.stringify(jqXHR));
        })
        .always(function () {
            index.loading(false);
        });
    }
};

requirejs.config({
    baseUrl: URL.base + 'lib',
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

ko.components.register('actions', {
    viewModel: { require: URL.base + '/js/Components/actions.js' },
    template: { require: 'text!../js/Templates/actions.html' }
});