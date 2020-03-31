URL = {

    base: window.location.href,
    apiBase: $('#apiBaseUrl').attr('data-stuff-url'),
    limit: 12,

    getNext: function () { return this.apiBase + "library/getNext" },
    getSeries: function (id) { return this.apiBase + "library/getSeries?seriesId=" + id + "&limit=" + this.limit },
    abandonSeries: function (id) { return this.apiBase + "library/abandonSeries?id=" + id },
    reinstateSeries: function (id) { return this.apiBase + "library/reinstateSeries?id=" + id },
    removeFromLibrary: function (id) { return this.apiBase + "library/removeFromLibrary?id=" + id },
    addToLibrary: function () { return this.apiBase + "library/addToLibrary" },
    getComics: function (id, offset) { return this.apiBase + "library/getComics?seriesId=" + id + "&limit=" + this.limit + "&offset=" + offset },
    getSeriesToRead: function () { return this.apiBase + "library/getSeriesToRead" },
    getSeriesFinished: function () { return this.apiBase + "library/getSeriesFinished" },
    getSeriesAbandoned: function () { return this.apiBase + "library/getSeriesAbandoned" },
    getSeriesInProgress: function () { return this.apiBase + "library/getSeriesInProgress" },
    markAsRead: function () { return this.apiBase + "library/markAsRead" },
    markAsUnread: function () { return this.apiBase + "library/markAsUnread" },
    
    searchByTitle: function (title, sortOrder, page) { return this.apiBase + "search/searchByTitle?title=" + title + "&sortOrder=" + sortOrder + "&limit=" + this.limit + "&page=" + page },
    getComicsByMarvelId: function (id, offset) { return this.apiBase + "search/getComicsByMarvelId?marvelId=" + id + "&limit=" + this.limit + "&offset=" + offset }
};