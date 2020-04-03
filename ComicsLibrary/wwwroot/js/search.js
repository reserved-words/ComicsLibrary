searchViewModel = {
    title: ko.observable(),
    sortOrder: ko.observable(),
    sortOrderOptions: [
        { id: 1, name: "Sort by title" },
        { id: 2, name: "Sort by year" }
    ],
    results: ko.observableArray(),
    page: ko.observable(),
    nextPage: ko.observable(),
    previousPage: ko.observable(),
    totalPages: ko.observable(),
    noResults: ko.observable(false),
    noCriteria: ko.observable(true),
    selectedSeries: ko.observable(),
    selectedId: ko.observable(),
    comics: ko.observableArray(),
    comicsPagesFetched: ko.observable(),
    totalComicsPages: ko.observable(),
    goToSeries: function(data, event) {
        index.loadSeries(data.id());
    },
    addToLibrary: function (data, event) {
        AJAX.post(URL.addToLibrary(), data, function (result) {
            if (result === 0)
            {
                alert("Error");
                return;
            }
            data.id(result);
        });
    }
};

searchViewModel.load = function ()
{
    this.clearSearch();
}

searchViewModel.clearSearch = function() {
    this.results.removeAll();
    this.title("");
    this.sortOrder(1);
    this.noResults(false);
    this.noCriteria(true);
}

searchViewModel.startSearch = function () {
    this.searchPage(1);
}

searchViewModel.goToPreviousPage = function () {
    this.searchPage(this.previousPage());
}

searchViewModel.goToNextPage = function () {
    this.searchPage(this.nextPage());
}

searchViewModel.searchPage = function (page) {
    var self = this;
    if (!self.title()) {
        alert("You must enter a title to search for");
        return;
    }
    self.noCriteria(false);

    AJAX.get(URL.searchByTitle(self.title(), self.sortOrder(), page), function (data) {
        self.totalPages(data.totalPages);
        self.page(data.page);
        self.nextPage(data.nextPage);
        self.previousPage(data.previousPage);
        self.results.removeAll();

        $(data.results).each(function (index, element) {
            self.results.push({
                id: ko.observable(element.id),
                title: element.title,
                url: element.url,
                imageUrl: element.imageUrl,
                type: element.type,
                marvelId: element.marvelId,
                issues: ko.observableArray(),
                pagesFetched: 0,
                totalPages: 0
            });
        });

        self.noResults(self.results().length === 0);
    });
}


searchViewModel.addSelectedSeriesToLibrary = function () {
    searchViewModel.addToLibrary(searchViewModel.selectedSeries(), null);
}

searchViewModel.goToSelectedSeries = function() {
    searchViewModel.goToSeries(searchViewModel.selectedSeries(), null);
}