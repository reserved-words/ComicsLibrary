currentViewModel = {
    collection: ko.observableArray(),
    comics: ko.observableArray(),
    selectedAction: ko.observable(0),
    onActionCompleted: function () {
        index.menuClick(index.menuItems[1], null);
    }
};

currentViewModel.load = function (id) {
    var self = this;
    self.selectedAction(0);
    AJAX.get(URL.get("getNext"), function (data) {
        self.collection.removeAll();
        $(data).each(function (index, element) {
            var series = {
                title: element.title,
                comics: ko.observableArray()
            };
            $(element.comics).each(function (index, element) {
                var comic = {
                    id: element.id,
                    readUrl: element.readUrl,
                    imageUrl: element.imageUrl,
                    isRead: element.isRead,
                    readNext: element.toReadNext,
                    title: element.issueTitle,
                    seriesTitle: null,
                    seriesId: element.seriesId,
                    selected: ko.observable(false),
                    select: function() {
                        this.selected(!this.selected());
                    }
                };
                series.comics.push(comic);
                self.comics.push(comic);
            });
            self.collection.push(series);
        });
    });
}
