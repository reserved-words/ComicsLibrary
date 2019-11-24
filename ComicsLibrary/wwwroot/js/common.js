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
            type: "GET",
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
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                onLoaded(result);
                index.loading(false);
            }
        });
    }
};

requirejs.config({
    baseUrl: URL.base + '/Scripts',
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