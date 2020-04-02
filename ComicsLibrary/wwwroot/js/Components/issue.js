define(['knockout'], function (ko) {

    function IssueViewModel(params) {

        this.id = params.id;
        this.title = params.title;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.onSaleDate = params.onSaleDate;
        this.isRead = params.isRead;

        this.markAsRead = function (data, event) {
            if (data.isRead)
                return;

            AJAX.post(URL.markAsRead(data.id), null, function () {
                data.isRead = true;
            });
        }
        this.markAsUnread = function (data, event) {
            if (!data.isRead)
                return;

            AJAX.post(URL.markAsUnread(data.id), null, function () {
                data.isRead = false;
            });
        }
    }

    return IssueViewModel;
});