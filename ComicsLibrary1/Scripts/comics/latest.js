latestViewModel = {
    added: ko.observableArray(),
    updated: ko.observableArray(),
    selectedAction: ko.observable(0),
    onActionCompleted: function () {
        index.menuClick(index.menuItems[0], null);
    }
};

latestViewModel.load = function (id) {
    var self = this;
    self.selectedAction(0);

    AJAX.get(URL.get('getUpdated'), function (data) {
        self.updated.removeAll();
        $(data).each(function (index, element) {
            self.updated.push({
                id: element.Id,
                readUrl: element.ReadUrl,
                imageUrl: element.ImageUrl,
                title: element.IssueTitle,
                seriesTitle: element.SeriesTitle,
                isRead: element.IsRead,
                readNext: element.ToReadNext,
                dateUpdated: element.ReadUrlAdded,
                seriesId: element.SeriesId,
                selected: ko.observable(false),
                select: function () {
                    this.selected(!this.selected());
                }
            });
        });
    });

    AJAX.get(URL.get('getNew'), function (data) {
        self.added.removeAll();
        $(data).each(function (index, element) {
            self.added.push({
                id: element.Id,
                readUrl: element.ReadUrl,
                imageUrl: element.ImageUrl,
                title: element.IssueTitle,
                seriesTitle: element.SeriesTitle,
                isRead: element.IsRead,
                readNext: element.ToReadNext,
                dateAdded: element.DateAdded,
                seriesId: element.SeriesId,
                selected: ko.observable(false),
                select: function () {
                    this.selected(!this.selected());
                }
            });
        });
    });
}