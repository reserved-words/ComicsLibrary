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
                title: element.Title,
                comics: ko.observableArray()
            };
            $(element.Comics).each(function (index, element) {
                var comic = {
                    id: element.Id,
                    readUrl: element.ReadUrl,
                    imageUrl: element.ImageUrl,
                    isRead: element.IsRead,
                    readNext: element.ToReadNext,
                    title: element.IssueTitle,
                    seriesTitle: null,
                    seriesId: element.SeriesId,
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
