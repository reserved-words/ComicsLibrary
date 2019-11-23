seriesViewModel = {
    id: ko.observable(),
    title: ko.observable(),
    subTitle: ko.observable(),
    comics: ko.observableArray(),
    isAbandoned: ko.observable(),
    totalComics: ko.observable(),
    selectedAction: ko.observable(0)
};

seriesViewModel.onActionCompleted = function () {
    index.loadSeries(seriesViewModel.id());
}

seriesViewModel.load = function (id) {
    var self = this;
    self.selectedAction(0);
    self.id(id);
    AJAX.get(URL.get('getSeries', id), function (data) {
        self.id(data.Id);
        self.title(data.MainTitle);
        self.subTitle(data.SubTitle);
        self.totalComics(data.TotalComics);
        self.isAbandoned(data.Abandoned);
        self.comics.removeAll();
        $(data.Comics).each(function (index, element) {
            self.addComic(element);
        });
    });
}

seriesViewModel.abandonSeries = function () {
    var self = this;
    AJAX.post(URL.get('abandonSeries', self.id()), null, function (result) {
        self.isAbandoned(true);
    });
}

seriesViewModel.reinstateSeries = function () {
    var self = this;
    AJAX.post(URL.get('reinstateSeries', self.id()), null, function (result) {
        self.isAbandoned(false);
    });
}

seriesViewModel.deleteSeries = function () {
    if (!confirm("Delete this series?"))
        return;
    var self = this;
    AJAX.post(URL.get('removeFromLibrary', self.id()), null, function (result) {
        index.loadSeries(0);
    });
}

seriesViewModel.getMoreComics = function () {
    AJAX.get(URL.get('getComics', seriesViewModel.id(), seriesViewModel.comics().length), function (data) {
        $(data).each(function (index, element) {
            seriesViewModel.addComic(element);
        });
    });
}

seriesViewModel.addComic = function(element){
    seriesViewModel.comics.push({
        id: element.Id,
        readUrl: element.ReadUrl,
        imageUrl: element.ImageUrl,
        isRead: element.IsRead,
        readNext: element.ToReadNext,
        title: element.IssueTitle,
        seriesTitle: null,
        selected: ko.observable(false),
        select: function () {
            this.selected(!this.selected());
        }
    });
}