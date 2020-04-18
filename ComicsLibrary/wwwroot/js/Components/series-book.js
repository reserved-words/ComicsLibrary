define(['knockout'], function (ko) {

    function IssueViewModel(params) {

        this.id = params.id;
        this.seriesId = params.seriesId;
        this.title = params.title;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.onSaleDate = params.onSaleDate;
        this.isRead = ko.observable(params.isRead);
        this.hidden = ko.observable(params.hidden);

        this.markAsRead = function (data, event) {
            update.markAsRead(data.id, data.seriesId, function () {
                data.isRead(true);
            });
        }

        this.markAsUnread = function (data, event) {
            update.markAsUnread(data.id, data.seriesId, function () {
                data.isRead(false);
            });
        }

        this.hide = function (data, event) {
            update.hideBook(data.id, data.seriesId, function () {
                data.hidden(true);
            });
        }

        this.unhide = function (data, event) {
            update.unhideBook(data.id, data.seriesId, function () {
                data.hidden(false);
            });
        }
    }

    return IssueViewModel;
});