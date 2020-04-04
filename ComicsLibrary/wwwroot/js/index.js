index = {
    loading: ko.observable(true),
    pages: [
        { name: "Home", isActive: ko.observable(true), viewModel: home, loaded: false, menu: true },
        { name: "Library", isActive: ko.observable(false), viewModel: library, loaded: false, menu: true },
        { name: "Search", isActive: ko.observable(false), viewModel: search, loaded: false, menu: true },
        { name: "Series", isActive: ko.observable(false), viewModel: series, loaded: false, menu: false }
    ],
    menuClick: function (data, event) {
        loadContent(data);
        setPageActive(data);
    },
    loadSeries: function (seriesId) {
        loadContent(index.pages[3], seriesId);
        setPageActive(index.pages[3]);
    }
};

var setPageActive = function(activePage) {
    $.each(index.pages, function (i, page) {
        page.isActive(false);
    });
    if (activePage) {
        activePage.isActive(true);
    }
}

var loadContent = function (page, id) {
    if (page.loaded && !id)
        return;

    page.viewModel.load(id);
    page.loaded = true;
}

$(document).on('click', '.navbar-collapse.in', function (e) {
    $(this).collapse('hide');
});

$(function ()
{
    ko.applyBindings(index);
    index.loading(true);
    var appBaseUrl = $('#appBaseUrl').data('stuff-url');

    $(index.pages).each(function (i, page) {
        $("#" + page.name).load(appBaseUrl + page.name + ".html", function () {
            ko.cleanNode($("#" + page.name)[0]);
            ko.applyBindings(page.viewModel, $("#" + page.name)[0]);

            if (i === 0) {
                index.menuClick(page, null);
            }
        });
    });
});