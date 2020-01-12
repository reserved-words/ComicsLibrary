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

    AJAX.get(URL.getUpdated(), function (data) {
        self.updated.removeAll();
        $(data).each(function (index, element) {
            self.updated.push({
                id: element.id,
                readUrl: element.readUrl,
                imageUrl: element.imageUrl,
                title: element.issueTitle,
                seriesTitle: element.seriesTitle,
                isRead: element.isRead,
                readNext: element.toReadNext,
                dateUpdated: element.readUrlAdded,
                seriesId: element.seriesId,
                selected: ko.observable(false),
                select: function () {
                    this.selected(!this.selected());
                }
            });
        });
    });

    AJAX.get(URL.getNew(), function (data) {
        self.added.removeAll();
        $(data).each(function (index, element) {
            self.added.push({
                id: element.id,
                readUrl: element.readUrl,
                imageUrl: element.imageUrl,
                title: element.issueTitle,
                seriesTitle: element.seriesTitle,
                isRead: element.isRead,
                readNext: element.toReadNext,
                dateAdded: element.dateAdded,
                seriesId: element.seriesId,
                selected: ko.observable(false),
                select: function () {
                    this.selected(!this.selected());
                }
            });
        });
    });
}