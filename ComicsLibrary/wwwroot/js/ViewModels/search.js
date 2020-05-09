search = {
    searchText: ko.observable(),
    sortOrder: ko.observable(),
    source: ko.observable(),
    sortOptions: ko.observableArray(),
    sourceOptions: ko.observableArray(),
    results: ko.observableArray(),
    totalPages: ko.observable(0),
    pagesFetched: ko.observable(0),
    noResults: ko.observable(false),
    noCriteria: ko.observable(true),
    getMoreResults: function ()
    {
        this.searchPage(this.pagesFetched() + 1);
    },
    sourceChanged: function () {
        var self = this;
        self.sortOptions.removeAll();

        $(self.source().sortOptions).each(function (i, v) {
            self.sortOptions.push({
                id: v.id,
                name: v.name
            });
        });

        self.sortOrder(self.sortOptions[0]);
    }
};

search.load = function () {
    index.loading(true);

    search.clearSearch();

    API.get(URL.getSearchOptions(), function (data) {
        $(data).each(function (index, element) {
            search.sourceOptions.push({
                id: element.id,
                name: element.name,
                sortOptions: element.sortOptions
            });
        });
        search.source(self.sourceOptions[0]);
        search.sourceChanged();

        index.loading(false);
    });
}

search.clearSearch = function() {
    this.results.removeAll();
    this.noResults(false);
    this.noCriteria(true);
    this.sortOptions.removeAll();
    this.sourceOptions.removeAll();
}

search.startSearch = function () {
    this.searchPage(1);
}

search.searchPage = function (page) {
    var self = this;

    if (!self.searchText()) {
        self.noCriteria(true);
        return;
    }

    self.noCriteria(false);

    if (page === 1) {
        self.results.removeAll();
    }

    API.get(URL.searchByTitle(self.source().id, self.searchText(), self.sortOrder().id, page), function (data) {

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