homeViewModel = {
    comics: ko.observableArray()
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