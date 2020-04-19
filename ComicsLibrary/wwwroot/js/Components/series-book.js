define(['knockout'], function () {

    function SeriesBookViewModel(params) {

        this.id = params.id;
        this.seriesId = params.seriesId;
        this.title = params.title;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.onSaleDate = params.onSaleDate;
        this.isRead = ko.observable(params.isRead);
        this.hidden = ko.observable(params.hidden);

        this.markAsRead = function (data, event) {
            var self = this;
            update.markAsRead(data.id, data.seriesId, function () {
                self.isRead(true);
            });
        }

        this.markAsUnread = function (data, event) {
            var self = this;
            update.markAsUnread(self.id, self.seriesId, function () {
                self.isRead(false);
            });
        }

        this.hide = function (data, event) {
            var self = this;
            update.hideBook(self.id, self.seriesId, function () {
                self.hidden(true);
            });
        }

        this.unhide = function (data, event) {
            var self = this;
            update.unhideBook(self.id, self.seriesId, function () {
                self.hidden(false);
            });
        }
    }

    return SeriesBookViewModel;
});