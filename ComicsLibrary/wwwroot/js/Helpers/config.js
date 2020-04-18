app = {
    maxFetch: 12,
    baseUrl: $('#appBaseUrl').data('stuff-url'),
    apiBaseUrl: $('#apiBaseUrl').data('stuff-url'),
    getUrl: function (controller, action) {
        return this.baseUrl + controller + "/" + action;
    },
    authSettingsUrl: function () {
        return this.getUrl("Auth", "Settings");
    },
    callbackUrl: function () {
        return this.getUrl("home", "callback");
    },
    apiUrl: function (controller, action) {
        return this.apiBaseUrl + controller + "/" + action;
    }
}

requirejs.config({
    baseUrl: app.baseUrl + '/lib',
    waitSeconds: 200,
    paths: {
        'knockout': 'KnockoutJS/knockout-3.5.0',
        text: 'RequireJS/text'
    }
});

ko.components.register('home-book', {
    viewModel: { require: app.baseUrl + '/js/Components/home-book.js' },
    template: { require: 'text!../js/Templates/home-book.html' }
});

ko.components.register('series-book', {
    viewModel: { require: app.baseUrl + '/js/Components/series-book.js' },
    template: { require: 'text!../js/Templates/series-book.html' }
});

ko.components.register('search-result', {
    viewModel: { require: app.baseUrl + '/js/Components/search-result.js' },
    template: { require: 'text!../js/Templates/search-result.html' }
});