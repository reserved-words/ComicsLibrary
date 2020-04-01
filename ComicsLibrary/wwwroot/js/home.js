homeViewModel = {
    comics: ko.observableArray(),
    markAsRead: function (id) {
        var self = this;

        AJAX.post(URL.markAsRead(id), null, function (result) {
            var oldComic = self.comics().filter(c => c.seriesId === result.seriesId)[0];
            result.unreadIssues = oldComic.unreadIssues - 1;
            self.comics.replace(oldComic, result);
        });
    },
    archiveSeries: function (seriesId) {
        alert("Archive series: " + seriesId);
    }
};

homeViewModel.load = function (id) {
    var self = this;

    AJAX.get(URL.getNext(), function (data) {
        self.comics.removeAll();
        $(data).each(function (index, element) {
            self.comics.push({
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