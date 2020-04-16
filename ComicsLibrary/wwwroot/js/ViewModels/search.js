search = {
    searchText: ko.observable(),
    sortOrder: ko.observable(),
    sourceID: ko.observable(),
    sortOrderOptions: [
        { id: 1, name: "Sort by title" },
        { id: 2, name: "Sort by year" }
    ],
    sourceOptions: [
        { id: 1, name: "Marvel Unlimited" },
        { id: 2, name: "Alternative Source" }
    ],
    results: ko.observableArray(),
    totalPages: ko.observable(0),
    pagesFetched: ko.observable(0),
    noResults: ko.observable(false),
    noCriteria: ko.observable(true),
    getMoreResults: function ()
    {
        this.searchPage(this.pagesFetched() + 1);
    }
};

search.load = function () {
    this.clearSearch();
}

search.clearSearch = function() {
    this.results.removeAll();
    this.sortOrder(1);
    this.sourceID(1);
    this.noResults(false);
    this.noCriteria(true);
}

search.startSearch = function () {
    this.searchPage(1);
}

search.searchPage = function (page) {

    console.log(page);

    var self = this;

    if (!self.searchText()) {
        self.noCriteria(true);
        return;
    }

    self.noCriteria(false);

    if (page === 1) {
        self.results.removeAll();
    }

    API.get(URL.searchByTitle(self.sourceID(), self.searchText(), self.sortOrder(), page), function (data) {

        self.pagesFetched(data.page);
        self.totalPages(data.totalPages);

        $(data.results).each(function (index, element) {
            self.results.push({
                libraryId: element.libraryId,
                title: element.title,
                sourceItemId: element.sourceItemId,
                url: element.url,
                imageUrl: element.imageUrl,
                sourceId: element.sourceId
            });
        });

        self.noResults(self.results().length === 0);
    });
}