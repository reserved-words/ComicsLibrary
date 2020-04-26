index = {
    loading: ko.observable(false),
    pages: [
        { name: "Home", isActive: ko.observable(true), viewModel: home, menu: true },
        { name: "Library", isActive: ko.observable(false), viewModel: library, menu: true },
        { name: "Search", isActive: ko.observable(false), viewModel: search, menu: true },
        { name: "Series", isActive: ko.observable(false), viewModel: series, menu: false }
    ],
    menuClick: function (data, event) {
        setPageActive(data);
    },
    loadSeries: function (seriesId) {
        index.pages[3].viewModel.load(seriesId);
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

var onAuthorized = function () {
    ko.applyBindings(index);
    index.loading(true);

    $(index.pages).each(function (i, page) {
        $("#" + page.name).load(app.baseUrl + page.name + ".html", function () {
            ko.cleanNode($("#" + page.name)[0]);
            ko.applyBindings(page.viewModel, $("#" + page.name)[0]);
            page.viewModel.load();

            if (i === 0) {
                index.menuClick(page, null);
            }
        });
    });
}

$(document).on('click', '.navbar-collapse.in', function (e) {
    $(this).collapse('hide');
});

$(function ()
{
    authorize(onAuthorized);
});