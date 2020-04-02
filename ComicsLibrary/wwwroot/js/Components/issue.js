define(['knockout'], function (ko) {

    function IssueViewModel(params) {

        this.id = params.id;
        this.title = params.title;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.onSaleDate = params.onSaleDate;
        this.isRead = params.isRead;

        this.markAsRead = function (data, event) {
            homeViewModel.markAsRead(data.id);
        }
    }

    return IssueViewModel;
});