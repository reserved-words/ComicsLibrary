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

ko.components.register('comic', {
    viewModel: { require: app.baseUrl + '/js/Components/comic.js' },
    template: { require: 'text!../js/Templates/comic.html' }
});

ko.components.register('issue', {
    viewModel: { require: app.baseUrl + '/js/Components/issue.js' },
    template: { require: 'text!../js/Templates/issue.html' }
});

ko.components.register('result', {
    viewModel: { require: app.baseUrl + '/js/Components/result.js' },
    template: { require: 'text!../js/Templates/result.html' }
});