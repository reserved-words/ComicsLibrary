index = {
    menuItems: [
        { id: "latest", name: "Recently Added", viewModel: latestViewModel, active: ko.observable(true) },
        { id: "current", name: "Read Next", viewModel: currentViewModel, active: ko.observable(false) },
        { id: "library", name: "In Progress", viewModel: libraryViewModel, active: ko.observable(false) },
        { id: "library", name: "To Read", viewModel: libraryViewModel, active: ko.observable(false) },
        { id: "library", name: "Finished", viewModel: libraryViewModel, active: ko.observable(false) },
        { id: "library", name: "Abandoned", viewModel: libraryViewModel, active: ko.observable(false) },
        { id: "search", name: "Search", viewModel: searchViewModel, active: ko.observable(false) }
    ],
    menuClick: function (data, event) {
        setMenuItemActive(data);
        loadContent(URL.getView(data.id), data.viewModel, data.name);
    },
    loading: ko.observable(true),
    loadSeries: function (id) {
        setMenuItemActive(null);
        loadContent(URL.getView("series", id), seriesViewModel, id);
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

var loadContent = function (viewUrl, viewModel, id) {
    $("#content").load(viewUrl, function() {
        viewModel.load(id);
        ko.cleanNode($('#content')[0]);
        ko.applyBindings(viewModel, $('#content')[0]);
        index.loading(false);
    });
}

$(document).on('click', '.navbar-collapse.in', function (e) {
    $(this).collapse('hide');
});