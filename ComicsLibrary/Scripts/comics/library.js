libraryViewModel = {
    title: ko.observable(),
    series: ko.observableArray(),
    viewSeries: function (data, event) {
        index.loadSeries(data.Id);
    }
};

libraryViewModel.load = function (id) {
    var self = this;

    if (!id) {
        id = self.title();
        if (!id) {
            id = "In Progress";
        }
    }

    self.title(id);

    switch (id) {
        case "To Read":
            url = URL.get("getSeriesToRead");
            break;
        case "Finished":
            url = URL.get("getSeriesFinished");
            break;
        case "Abandoned":
            url = URL.get("getSeriesAbandoned");
            break;
        default:
            url = URL.get("getSeriesInProgress");
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
    AJAX.post(URL.get("abandonSeries", data.Id), null, function (result) {
        libraryViewModel.load();
    });
}

libraryViewModel.reinstateSeries = function (data, event) {
    AJAX.post(URL.get("reinstateSeries", data.Id), null, function (result) {
        libraryViewModel.load();
    });
}

libraryViewModel.deleteSeries = function (data, event) {
    if (!confirm("Delete this series?"))
        return;
    AJAX.post(URL.get("removeFromLibrary", data.Id), null, function (result) {
        libraryViewModel.load();
    });
}