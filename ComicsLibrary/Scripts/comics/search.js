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
        AJAX.post(URL.get('addToLibrary'), data, function (result) {
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
    AJAX.get(URL.get("searchByTitle", '', '', page, self.sortOrder(), self.title()), function (data) {
        self.totalPages(data.TotalPages);
        self.page(data.Page);
        self.nextPage(data.NextPage);
        self.previousPage(data.PreviousPage);
        self.results.removeAll();
        $(data.Results).each(function (index, element) {
            self.results.push({
                id: ko.observable(element.Id),
                title: element.Title,
                startYear: element.StartYear,
                endYear: element.EndYear,
                url: element.Url,
                imageUrl: element.ImageUrl,
                type: element.Type,
                marvelId: element.MarvelId
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
    if (!searchViewModel.selectedSeries().marvelId)
        return;
    
    var url = URL.get('getComicsByMarvelId', searchViewModel.selectedSeries().marvelId, searchViewModel.comics().length);
    AJAX.get(url, function (result) {
        searchViewModel.comicsPagesFetched(result.Page);
        searchViewModel.totalComicsPages(result.TotalPages);
        $(result.Results).each(function (index, element) {
            searchViewModel.comics.push({
                title: element.IssueTitle,
                imageUrl: element.ImageUrl,
                readUrl: element.ReadUrl
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