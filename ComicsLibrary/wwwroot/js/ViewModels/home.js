home = {
    comics: ko.observableArray(),
    markAsRead: function (id) {
        AJAX.post(URL.markAsRead(id), null, function (result) {
            var oldComic = home.comics().filter(c => c.seriesId === result.seriesId)[0];
            result.unreadIssues = oldComic.unreadIssues - 1;
            home.comics.replace(oldComic, result);
        });
    },
    archiveSeries: function (seriesId) {
        AJAX.post(URL.abandonSeries(seriesId), null, function (result) {
            home.comics.remove(item => item.seriesId === seriesId);
        });
    },
    load: function () {
        AJAX.get(URL.getNext(), function (data) {
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
                    unreadIssues: element.unreadIssues
                });
            });
        });
    }
};