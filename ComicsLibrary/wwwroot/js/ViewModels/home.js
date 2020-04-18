home = {
    comics: ko.observableArray(),
    load: function () {
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
                    onSaleDate: element.onSaleDate,
                    unreadIssues: element.unreadIssues,
                    creators: element.creators
                });
            });
        });
    },
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
            if (!result) {
                home.comics.remove(oldComic);
            }
            else {
                home.comics.replace(oldComic, result);
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
            if (element) {
                var item = {
                    id: element.id,
                    seriesTitle: element.seriesTitle,
                    issueTitle: element.issueTitle,
                    imageUrl: element.imageUrl,
                    readUrl: element.readUrl,
                    seriesId: element.seriesId,
                    onSaleDate: element.onSaleDate,
                    unreadIssues: element.unreadIssues,
                    creators: element.creators
                };

                for (var i in home.comics()) {
                    if (home.comics()[i].seriesTitle > item.seriesTitle) {
                        home.comics.splice(i, 0, item);
                        return false;
                    }
                }
                home.comics.push(item);
            }
        });
    }
};