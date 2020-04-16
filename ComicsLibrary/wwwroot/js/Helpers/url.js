URL = {
    getSearchOptions: () =>
        app.apiUrl("search", "getSearchOptions"),

    getNext: () =>
        app.apiUrl("library", "getNext"),

    getBooks: (id, typeId, offset) =>
        app.apiUrl("library", "getBooks")
            .concat("?", "seriesId", "=", id)
            .concat("&", "typeId", "=", typeId)
            .concat("&", "limit", "=", app.maxFetch)
            .concat("&", "offset", "=", offset),

    getSeries: (id) =>
        app.apiUrl("library", "getSeries")
            .concat("?", "seriesId", "=", id)
            .concat("&", "limit",    "=", app.maxFetch),

    abandonSeries: (id) =>
        app.apiUrl("library", "abandonSeries")
            .concat("?", "id", "=", id),

    reinstateSeries: (id) =>
        app.apiUrl("library", "reinstateSeries")
            .concat("?", "id", "=", id),

    removeFromLibrary: (id) =>
        app.apiUrl("library", "removeFromLibrary")
            .concat("?", "id", "=", id),

    addToLibrary: () =>
        app.apiUrl("search", "addToLibrary"),

    getComics: (id, offset) =>
        app.apiUrl("library", "getComics")
            .concat("?", "seriesId", "=", id)
            .concat("&", "limit",    "=", app.maxFetch)
            .concat("&", "offset",   "=", offset),

    getSeriesByStatus: (id) =>
        app.apiUrl("library", "getSeriesByStatus")
            .concat("?", "status", "=", id),

    markAsRead: (id) =>
        app.apiUrl("library", "markAsRead")
            .concat("?", "id", "=", id),

    setHomeOption: () =>
        app.apiUrl("library", "setHomeOption"),

    markAsUnread: (id) =>
        app.apiUrl("library", "markAsUnread")
            .concat("?", "id", "=", id),

    searchByTitle: (sourceID, title, sortOrder, page) =>
        app.apiUrl("search", "searchByTitle")
            .concat("?", "sourceID",  "=", sourceID)
            .concat("&", "title",     "=", title)
            .concat("&", "sortOrder", "=", sortOrder)
            .concat("&", "limit",     "=", app.maxFetch)
            .concat("&", "page",      "=", page),

    getComicsByMarvelId: (id, offset) =>
        app.apiUrl("search", "getComicsByMarvelId")
            .concat("?", "marvelId", "=", id)
            .concat("&", "limit",    "=", app.maxFetch)
            .concat("&", "offset",   "=", offset)
};