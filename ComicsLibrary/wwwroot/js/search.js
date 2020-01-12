searchViewModel = {
    title: ko.observable(),
    sortOrder: ko.observable(),
    sortOrderOptions: [
        { id: 1, name: "Title Ascending" },
        { id: 2, name: "Start Year Descending" }
    ],
    results: ko.observableArray(),
    page: ko.observable(),
    nextPage: ko.observable(),
    previousPage: ko.observable(),
    totalPages: ko.observable(),
    noResults: ko.observable(false),
    noCriteria: ko.observable(true),
    selectedSeries: ko.observable(),
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
    this.selectSeries(null);
}

searchViewModel.startSearch = function () {
    this.selectSeries(null);
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
                startYear: element.startYear,
                endYear: element.endYear,
                url: element.url,
                imageUrl: element.imageUrl,
                type: element.type,
                marvelId: element.marvelId
            });
        });
        self.noResults(self.results().length === 0);
    });
}

searchViewModel.selectSeries = function (data, event) {
    searchViewModel.comics.removeAll();
    if (!data) {
        data = {
            id: ko.observable(0),
            title: null
        }
    }
    searchViewModel.selectedSeries(data);
    searchViewModel.getMoreComics();
}

searchViewModel.getMoreComics = function () {
    if (!searchViewModel.selectedSeries().marvelId) {
        return;
    }
    
    var url = URL.getComicsByMarvelId(searchViewModel.selectedSeries().marvelId, searchViewModel.comics().length);
    AJAX.get(url, function (result) {
        searchViewModel.comicsPagesFetched(result.page);
        searchViewModel.totalComicsPages(result.totalPages);
        $(result.results).each(function (index, element) {
            searchViewModel.comics.push({
                title: element.issueTitle,
                imageUrl: element.imageUrl,
                readUrl: element.readUrl
            });
        });
    });
}

searchViewModel.addSelectedSeriesToLibrary = function () {
    searchViewModel.addToLibrary(searchViewModel.selectedSeries(), null);
}

searchViewModel.goToSelectedSeries = function() {
    searchViewModel.goToSeries(searchViewModel.selectedSeries(), null);
}