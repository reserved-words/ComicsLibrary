
library = {
    shelves: [
        { id: 0, title: "Reading", items: ko.observableArray(), selected: ko.observable(true), loaded: false },
        { id: 1, title: "New", items: ko.observableArray(), selected: ko.observable(false), loaded: false },
        { id: 2, title: "Finished", items: ko.observableArray(), selected: ko.observable(false), loaded: false },
        { id: 3, title: "Archived", items: ko.observableArray(), selected: ko.observable(false), loaded: false }
    ],
    select: function (data, event) {
        library.setSelected(data.id);
    },
    archiveSeries: function (data, event) {
        update.archiveSeries(data.id);
    },
    reinstateSeries: function (data, event) {
        update.reinstateSeries(data.id);
    },
    deleteSeries: function (data, event) {
        update.deleteSeries(data.id);
    },
    goToSeries: function (data, event) {
        index.loadSeries(data.id);
    },
    onBookStatusUpdated: function (seriesId) {
        var result = library.find(seriesId);
        if (!result.series) {
            library.onSeriesAdded(seriesId);
        }
        else {
            API.get(URL.getProgress(seriesId), function (value) {
                result.series.progress(value);
                library.move(result.series, result.shelf);
            });
        }
    },
    onSeriesArchived: function (seriesId) {
        var result = this.find(seriesId);
        if (!result.series) {
            library.onSeriesAdded(seriesId);
        }
        else {
            result.series.abandoned = true;
            library.move(result.series, result.shelf);
        }
    },
    onSeriesReinstated: function (seriesId) {
        var result = this.find(seriesId);
        if (!result.series) {
            library.onSeriesAdded(seriesId);
        }
        else {
            result.series.abandoned = false;
            library.move(result.series, result.shelf);
        }
    },
    onSeriesAdded: function (seriesId) {
        API.get(URL.getLibrarySeries(seriesId, 0), function (element) {
            var series = {
                id: element.id,
                title: element.title,
                imageUrl: element.imageUrl,
                abandoned: element.abandoned,
                progress: ko.observable(element.progress)
            }
            library.move(series, null);
        });
    },
    onSeriesDeleted: function (seriesId) {
        var result = this.find(seriesId);
        if (!result.series)
            return;
        result.shelf.items.remove(result.series);
    },
    move: function (item, oldShelf) {
        var newShelfId = library.getShelf(item);
        if (oldShelf) {
            if (oldShelf.id === newShelfId) {
                return;
            }   

            oldShelf.items.remove(item);
        }

        this.insertItem(newShelfId, item);
    },
    getShelf(series) {
        return series.abandoned
            ? 3
            : series.progress() === 100
                ? 2
                : series.progress() === 0
                    ? 1
                    : 0;
    },
    find: function (seriesId) {
        var foundShelf = null;
        var foundSeries = null;

        $(library.shelves).each(function (i, shelf) {
            if (foundShelf) {
                return false;
            }
            $(shelf.items()).each(function (j, series) {
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
    insertItem: function (shelfIndex, item) {
        var shelf = library.shelves[shelfIndex];

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

    if (!selectedShelf.loaded) {
        library.populateShelf(selectedShelf);
    }

    $.each(library.shelves, function (index, value) {
        value.selected(false);
    });

    selectedShelf.selected(true);

}

library.populateShelf = function (shelf) {
    index.loading(true);

    shelf.loaded = true;

    API.get(URL.getLibraryShelf(shelf.id), function (data) {

        $(data).each(function (index, series) {
            shelf.items.push({
                id: series.id,
                title: series.title,
                imageUrl: series.imageUrl,
                abandoned: series.archived,
                progress: ko.observable(series.progress)
            });
        });

        index.loading(false);
    });
}

library.load = function () {
    library.setSelected(0);
}