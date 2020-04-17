define(['knockout'], function (ko) {

    function IssueViewModel(params) {

        this.id = params.id;
        this.title = params.title;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.onSaleDate = params.onSaleDate;
        this.isRead = ko.observable(params.isRead);
        this.hidden = params.hidden;

        this.markAsRead = function (data, event) {
            if (data.isRead)
                return;

            API.post(URL.markAsRead(data.id), null, function () {
                data.isRead(true);
            });
        }

        this.markAsUnread = function (data, event) {
            if (!data.isRead)
                return;

            API.post(URL.markAsUnread(data.id), null, function () {
                data.isRead(false);
            });
        }

        this.hide = function (data, event) {
            var id = data.id;
            API.post(URL.hideBook(id), null, function () {
                series.hideBook(id, true);
            });
        }

        this.unhide = function (data, event) {
            var id = data.id;
            API.post(URL.unhideBook(id), null, function () {
                series.hideBook(id, false);
            });
        }
    }

    return IssueViewModel;
});