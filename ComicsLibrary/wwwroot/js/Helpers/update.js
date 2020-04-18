update = {

    markAsRead: function (bookId, seriesId, onSuccess) {
        API.post(URL.markAsRead(bookId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            home.onBookStatusUpdated(seriesId);
            library.onBookRead(seriesId);
        });
    },

    markAsUnread: function (bookId, seriesId, onSuccess) {
        API.post(URL.markAsUnread(bookId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            home.onBookStatusUpdated(seriesId);
            library.onBookUnread(seriesId);
        });
    },

    hideBook: function (bookId, seriesId, onSuccess) {
        API.post(URL.hideBook(bookId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            series.hideBook(bookId, true);
            home.onBookStatusUpdated(seriesId);
            library.onBookHidden(bookId, seriesId);
        });
    },

    unhideBook: function (bookId, seriesId, onSuccess) {
        API.post(URL.unhideBook(bookId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            series.hideBook(bookId, false);
            home.onBookStatusUpdated(seriesId);
            library.onBookUnhidden(bookId, seriesId);
        });
    },

    archiveSeries: function (seriesId, onSuccess) {
        API.post(URL.abandonSeries(seriesId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            home.onSeriesArchived(seriesId);
            library.onSeriesArchived(seriesId);
        });
    },

    reinstateSeries: function (seriesId, onSuccess) {
        API.post(URL.reinstateSeries(seriesId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            home.onSeriesReinstated(seriesId);
            library.onSeriesReinstated(seriesId);
        });
    },

    deleteSeries: function (seriesId, onSuccess) {
        if (!confirm("Are you sure you want to delete this series?"))
            return;

        API.post(URL.deleteSeries(seriesId), null, function () {
            if (onSuccess) {
                onSuccess();
            }
            library.onSeriesDeleted(seriesId);
            home.onSeriesDeleted(seriesId);
        });
    },

    addSeries: function (data, onSuccess) {
        API.post(URL.addToLibrary(), data, function (newId) {
            if (onSuccess) {
                onSuccess(newId);
            }
            home.onSeriesAdded(newId);
            library.onSeriesAdded(newId);
        });
    },

    updateHomeOption: function (data, onSuccess) {
        API.post(URL.setHomeOption(), data, function () {
            if (onSuccess) {
                onSuccess(newId);
            }
            home.onBookStatusUpdated(seriesId);
            library.onHomeOptionUpdated(seriesId);
        });
    }
}