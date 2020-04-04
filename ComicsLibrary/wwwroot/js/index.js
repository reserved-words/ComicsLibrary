index = {
    menuItems: [
        { id: "home", name: "Home", viewModel: home, active: ko.observable(true) },
        { id: "library", name: "Library", viewModel: library, active: ko.observable(true) },
        { id: "search", name: "Search", viewModel: search, active: ko.observable(false) }
    ],
    menuClick: function (data, event) {
        setMenuItemActive(data);
        loadContent(data.name, data.viewModel, data.name);
    },
    loading: ko.observable(true),
    loadSeries: function (seriesId) {
        setMenuItemActive(null);
        loadContent("series", series, seriesId);
    }
};

var setMenuItemActive = function(activeItem) {
    $.each(index.menuItems, function (i, element) {
        element.active(false);
    });
    if (activeItem) {
        activeItem.active(true);
    }
}

var loadContent = function (name, viewModel, id) {
    index.loading(true);
    var appBaseUrl = $('#appBaseUrl').data('stuff-url');
    $("#content").load(appBaseUrl + name + ".html", function() {
        viewModel.load(id);
        ko.cleanNode($('#content')[0]);
        ko.applyBindings(viewModel, $('#content')[0]);
        index.loading(false);
    });
}

$(document).on('click', '.navbar-collapse.in', function (e) {
    $(this).collapse('hide');
});

$(function ()
{
    ko.applyBindings(index);
    index.loading(false);
    index.menuClick(index.menuItems[0], null);
});