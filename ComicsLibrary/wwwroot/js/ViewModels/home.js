home = {
    comics: ko.observableArray(),
    onBookStatusUpdated: function (seriesId) {
        this.updateSeries(seriesId);
    },
    onSeriesArchived: function (id) {
        this.removeSeries(id);
    },
    onSeriesDeleted: function (id) {
        this.removeSeries(id);
    },
    updateSeries: function (seriesId) {
        API.get(URL.getNextInSeries(seriesId), function (result) {

            var oldComic = home.comics().filter(c => c.seriesId === seriesId)[0];
            if (oldComic) {
                home.comics.remove(oldComic);
            }

            if (result) {
                home.insertSeries(result);
            }

        });
    },
    onSeriesAdded: function (id) {
        this.addSeries(id);
    },
    onSeriesReinstated: function (id) {
        this.addSeries(id);
    },
    removeSeries: function (id) {
        home.comics.remove(item => item.seriesId === id);
    },
    addSeries: function (id) {
        API.get(URL.getNextInSeries(id), function (element) {
            home.insertSeries(element);
        });
    },
    insertSeries: function (element) {
        if (!element)
            return;

        var item = {
            id: element.id,
            seriesTitle: element.seriesTitle,
            issueTitle: element.issueTitle,
            imageUrl: element.imageUrl,
            readUrl: element.readUrl,
            seriesId: element.seriesId,
            unreadBooks: element.unreadBooks,
            creators: element.creators,
            progress: element.progress
        };

        for (var i in home.comics()) {
            if (home.comics()[i].seriesTitle > item.seriesTitle) {
                home.comics.splice(i, 0, item);
                return false;
            }
        }
        home.comics.push(item);
    }
};

home.load = function () {
    index.loading(true);

    API.get(URL.getNext(), function (data) {
        home.comics.removeAll();
        $(data).each(function (index, element) {
            home.comics.push({
                id: element.id,
                seriesTitle: element.seriesTitle,
                issueTitle: element.issueTitle,
                imageUrl: element.imageUrl,
                readUrl: element.readUrl,
                seriesId: element.seriesId,
                unreadBooks: element.unreadBooks,
                creators: element.creators,
                progress: element.progress
            });
        });
        index.loading(false);
    });
}