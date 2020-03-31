homeViewModel = {
    comics: ko.observableArray(),
    // selectedAction: ko.observable(0)
    //,
    //onActionCompleted: function () {
    //    index.menuClick(index.menuItems[0], null);
    //}
};

homeViewModel.load = function (id) {
    var self = this;
//    self.selectedAction(0);

    AJAX.get(URL.getNext(), function (data) {
        self.comics.removeAll();
        $(data).each(function (index, element) {
            self.comics.push({
                id: element.id,
                readUrl: element.readUrl,
                imageUrl: element.imageUrl,
                title: element.issueTitle,
                seriesTitle: element.seriesTitle,
                //isRead: element.isRead,
                //readNext: element.toReadNext,
                //dateAdded: element.dateAdded,
                seriesId: element.seriesId,
                //selected: ko.observable(false),
                //select: function () {
                //    this.selected(!this.selected());
                //}
            });
        });
    });
}