series = {
    id: ko.observable(),
    title: ko.observable(),
    issues: ko.observableArray(),
    isAbandoned: ko.observable(),
    totalIssues: ko.observable()
};

// Add methods to archive / reinstate / delete series

series.load = function (id) {
    var self = this;

    self.id(id);
    self.title("");
    self.issues.removeAll();
    self.isAbandoned(false);
    self.totalIssues(0);

    AJAX.get(URL.getSeries(id), function (data) {
        self.id(data.id);
        self.title(data.title);
        self.totalIssues(data.totalComics);
        self.isAbandoned(data.abandoned);
        self.issues.removeAll();
        $(data.issues).each(function (index, element) {
            self.addIssue(element);
        });
    });
}

series.abandonSeries = function () {
    var self = this;
    AJAX.post(URL.abandonSeries(self.id()), null, function (result) {
        self.isAbandoned(true);
    });
}

series.reinstateSeries = function () {
    var self = this;
    AJAX.post(URL.reinstateSeries(self.id()), null, function (result) {
        self.isAbandoned(false);
    });
}

series.deleteSeries = function () {
    if (!confirm("Delete this series?"))
        return;
    var self = this;
    AJAX.post(URL.removeFromLibrary(self.id()), null, function (result) {
        index.loadSeries(0);
    });
}

series.getMoreIssues = function () {
    AJAX.get(URL.getComics(series.id(), series.issues().length), function (data) {
        $(data).each(function (index, element) {
            series.addIssue(element);
        });
    });
}

series.addIssue = function (element) {
    series.issues.push({
        id: element.id,
        readUrl: element.readUrl,
        imageUrl: element.imageUrl,
        isRead: element.isRead,
        title: element.issueTitle,
        onSaleDate: element.onSaleDate
    });
}