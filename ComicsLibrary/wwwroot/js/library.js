libraryViewModel = {
    title: ko.observable(),
    series: ko.observableArray(),
    viewSeries: function (data, event) {
        index.loadSeries(data.id);
    }
};

libraryViewModel.load = function (id) {
    var self = this;

    self.title("");
    self.series.removeAll();

    if (!id) {
        id = self.title();
        if (!id) {
            id = "In Progress";
        }
    }

    self.title(id);

    switch (id) {
        case "To Read":
            url = URL.getSeriesToRead();
            break;
        case "Finished":
            url = URL.getSeriesFinished();
            break;
        case "Abandoned":
            url = URL.getSeriesAbandoned();
            break;
        default:
            url = URL.getSeriesInProgress();
            break;
    }

    AJAX.get(url, function (data) {
        self.series.removeAll();
        $(data).each(function (index, element) {
            self.series.push(element);
        });
    });
}

libraryViewModel.abandonSeries = function (data, event) {
    AJAX.post(URL.abandonSeries(data.id), null, function (result) {
        libraryViewModel.load();
    });
}

libraryViewModel.reinstateSeries = function (data, event) {
    AJAX.post(URL.reinstateSeries(data.id), null, function (result) {
        libraryViewModel.load();
    });
}

libraryViewModel.deleteSeries = function (data, event) {
    if (!confirm("Delete this series?"))
        return;

    AJAX.post(URL.removeFromLibrary(data.id), null, function (result) {
        libraryViewModel.load();
    });
}