library = {
    shelves: [
        { id: 1, title: "Reading", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 2, title: "To Read", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 3, title: "Read", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 4, title: "Archived", items: ko.observableArray(), fetched: false, selected: ko.observable(false) }
    ],
    select: function (data, event) {
        library.setSelected(data.id);
    },
    archiveSeries: function (data, event) {
        API.post(URL.abandonSeries(data.id), null, function () {
            library.onSeriesArchived(data.id);
        });
    },
    reinstateSeries: function (data, event) {
        API.post(URL.reinstateSeries(data.id), null, function () {
            library.onSeriesReinstated(data.id);
        });
    },
    deleteSeries: function (data, event) {
        if (!confirm("Delete this series?"))
            return;

        API.post(URL.deleteSeries(data.id), null, function () {
            library.onSeriesDeleted(data.id);
        });
    },
    goToSeries: function (data, event) {
        index.loadSeries(data.id);
    },
    onIssueRead: function (seriesId) {
        var result = this.find(seriesId);
        result.series.unreadIssues--;
        this.move(result.series, result.shelf, newShelf);
    },
    onIssueUnread: function (seriesId) {
        var result = this.find(seriesId);
        result.series.unreadIssues++;
        this.move(result.series, result.shelf);
    },
    onSeriesArchived: function (seriesId) {
        var result = this.find(seriesId);
        result.series.abandoned = true;
        this.move(result.series, result.shelf);
    },
    onSeriesReinstated: function (seriesId) {
        var result = this.find(seriesId);
        result.series.abandoned = false;
        this.move(result.series, result.shelf);
    },
    onSeriesAdded: function (series) {
        this.move(series, null, newShelf);
    },
    onSeriesDeleted: function (seriesId) {
        var result = this.find(seriesId);
        result.shelf.items.remove(result.series);
    },
    move: function (item, oldShelf) {
        var newShelf = this.getShelf(series);

        if (oldShelf.id === newShelf.id)
            return;

        if (oldShelf) {
            oldShelf.items.remove(item);
        }

        this.insertItem(newShelf, item);
    },
    getShelf(series) {
        return series.abandoned
            ? 3
            : series.unreadIssues === 0
                ? 2
                : series.unreadIssues === series.totalComics
                    ? 1
                    : 0;
    },
    find: function (seriesId) {
        var foundShelf = null;
        var foundSeries = null;

        $.each(library.shelves, function (i, shelf) {
            if (foundShelf) {
                return false;
            }
            $.each(shelf.items(), function (j, series) {
                if (series.id === seriesId) {
                    foundShelf = shelf;
                    foundSeries = series;
                    return false;
                }
            });
        });

        return {
            shelf: foundShelf,
            series: foundSeries
        };
    },
    insertItem: function (shelf, item) {
        for (var i in shelf.items()) {
            if (shelf.items()[i].title > item.title) {
                shelf.items.splice(i, 0, item);
                return false;
            }
        }
        shelf.items.push(item);
    }
};

library.setSelected = function(selectedId){
    var selectedShelf = library.shelves.filter(s => s.id === selectedId)[0];

    $.each(library.shelves, function (index, value) {
        value.selected(false);
    });

    selectedShelf.selected(true);

    if (selectedShelf.fetched)
        return;

    API.get(URL.getSeriesByStatus(selectedId), function (data) {
        selectedShelf.items.removeAll();
        selectedShelf.fetched = true;
        $(data).each(function (index, element) {
            selectedShelf.items.push({
                id: element.id,
                title: element.title,
                imageUrl: element.imageUrl,
                abandoned: element.abandoned,
                progress: element.progress,
                unreadIssues: element.unreadIssues,
                totalComics: element.totalComics
            });
        });
    });
}

library.load = function () {
    library.setSelected(1);
}