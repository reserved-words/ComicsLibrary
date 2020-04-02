seriesViewModel = {
    id: ko.observable(),
    title: ko.observable(),
    subTitle: ko.observable(),
    issues: ko.observableArray(),
    isAbandoned: ko.observable(),
    totalIssues: ko.observable()
};

seriesViewModel.load = function (id) {
    var self = this;
    self.id(id);
    AJAX.get(URL.getSeries(id), function (data) {
        self.id(data.id);
        self.title(data.mainTitle);
        self.subTitle(data.subTitle);
        self.totalIssues(data.totalIssues);
        self.isAbandoned(data.abandoned);
        self.issues.removeAll();
        $(data.issues).each(function (index, element) {
            self.addIssue(element);
        });
    });
}

seriesViewModel.abandonSeries = function () {
    var self = this;
    AJAX.post(URL.abandonSeries(self.id()), null, function (result) {
        self.isAbandoned(true);
    });
}

seriesViewModel.reinstateSeries = function () {
    var self = this;
    AJAX.post(URL.reinstateSeries(self.id()), null, function (result) {
        self.isAbandoned(false);
    });
}

seriesViewModel.deleteSeries = function () {
    if (!confirm("Delete this series?"))
        return;
    var self = this;
    AJAX.post(URL.removeFromLibrary(self.id()), null, function (result) {
        index.loadSeries(0);
    });
}

seriesViewModel.getMoreIssues = function () {
    AJAX.get(URL.getComics(seriesViewModel.id(), seriesViewModel.issues().length), function (data) {
        $(data).each(function (index, element) {
            seriesViewModel.addIssue(element);
        });
    });
}

seriesViewModel.addIssue = function(element){
    seriesViewModel.issues.push({
        id: element.id,
        readUrl: element.readUrl,
        imageUrl: element.imageUrl,
        isRead: element.isRead,
        title: element.issueTitle,
        onSaleDate: element.onSaleDate
    });
}