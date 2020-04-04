libraryViewModel = {
    shelves: [
        { id: 1, title: "Reading", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 2, title: "To Read", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 3, title: "Read", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 4, title: "Archived", items: ko.observableArray(), fetched: false, selected: ko.observable(false) }
    ],
    select: function (data, event) {
        libraryViewModel.setSelected(data.id);
    }
};

// Add methods to move series from one section to another
// When comic is marked as read and there are no more unread in the series - move to Read
// When comic is marked as unread - move to Reading / To Read
// When comic is archived from a different section - move to Archived
// When a new series is added - move to ToRead

libraryViewModel.setSelected = function(selectedId){
    var selectedShelf = libraryViewModel.shelves.filter(s => s.id === selectedId)[0];

    $.each(libraryViewModel.shelves, function (index, value) {
        value.selected(false);
    });

    selectedShelf.selected(true);

    if (selectedShelf.fetched)
        return;

    AJAX.get(URL.getSeriesByStatus(selectedId), function (data) {
        selectedShelf.items.removeAll();
        selectedShelf.fetched = true;
        $(data).each(function (index, element) {
            selectedShelf.items.push({
                id: element.id,
                title: element.title,
                imageUrl: element.imageUrl,
                abandoned: element.abandoned,
                progress: element.progress
            });
        });
    });
}

libraryViewModel.load = function () {
    libraryViewModel.setSelected(1);
}

libraryViewModel.goToSeries = function (data, event) {
    index.loadSeries(data.id);
}

libraryViewModel.archiveSeries = function (data, event) {
    AJAX.post(URL.abandonSeries(data.id), null, function () {
        data.abandoned = true;
        $(libraryViewModel.shelves).each(function (index, element) {
            element.items.remove(item => item.id === data.id);
        });

        libraryViewModel.insertAlphabetically(libraryViewModel.shelves[3], data);
    });
}

libraryViewModel.deleteSeries = function (data, event) {
    if (!confirm("Delete this series?"))
        return;

    AJAX.post(URL.deleteSeries(seriesId), null, function (result) {
        $(libraryViewModel.shelves).each(function (index, element) {
            element.items.remove(item => item.id === data.id);
        });
    });
}

libraryViewModel.reinstateSeries = function (data, event) {
    AJAX.post(URL.reinstateSeries(data.id), null, function () {

        data.abandoned = false;

        libraryViewModel.shelves[3].items.remove(item => item.id === data.id);

        var newShelf = data.progress === 0 ? 1 : data.progress === 100 ? 2 : 0;

        libraryViewModel.insertAlphabetically(libraryViewModel.shelves[newShelf], data);
    });
}

libraryViewModel.insertAlphabetically = function (shelf, newItem) {
    for (var i in shelf.items()) {
        if (shelf.items()[i].title > newItem.title) {
            shelf.items.splice(i, 0, newItem);
            return;
        }
    }

    shelf.items.push(newItem);
}