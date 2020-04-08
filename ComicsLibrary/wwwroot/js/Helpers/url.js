URL = {
    getNext: () =>
        app.apiUrl("library", "getNext"),

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
        app.apiUrl("library", "addToLibrary"),

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

    markAsUnread: (id) =>
        app.apiUrl("library", "markAsUnread")
            .concat("?", "id", "=", id),

    searchByTitle: (title, sortOrder, page) =>
        app.apiUrl("search", "searchByTitle")
            .concat("?", "title",     "=", title)
            .concat("&", "sortOrder", "=", sortOrder)
            .concat("&", "limit",     "=", app.maxFetch)
            .concat("&", "page",      "=", page),

    getComicsByMarvelId: (id, offset) =>
        app.apiUrl("search", "getComicsByMarvelId")
            .concat("?", "marvelId", "=", id)
            .concat("&", "limit",    "=", app.maxFetch)
            .concat("&", "offset",   "=", offset)
};