define(['knockout'], function () {

    function HomeBookViewModel(params) {

        this.id = params.id;
        this.seriesTitle = params.seriesTitle;
        this.issueTitle = params.issueTitle;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.seriesId = params.seriesId;
        this.unreadBooks = params.unreadBooks;
        this.creators = params.creators;
        this.progress = params.progress;
        this.loading = ko.observable(false);

        var query = params.seriesTitle
            .concat(" ", params.issueTitle, " ", params.creators)
            .replace("(", "")
            .replace(")", "")
            .replace(/&/g, "")
            .replace("#", "%23")
            .replace(" ", "+");

        this.goodreadsUrl = "https://www.goodreads.com/search"
            + "?utf8=%E2%9C%93"
            + "&search_type=books"
            + "&search%5Bfield%5D=on"
            + "&q="
            + query;

        this.goToSeries = function(data,event) {
            index.loadSeries(data.seriesId);
        }

        this.markAsRead = function (data, event) {
            this.loading(true);
            update.markAsRead(data.id, data.seriesId);
        }

        this.archiveSeries = function (data, event) {
            update.archiveSeries(data.seriesId);
        }
    }

    return HomeBookViewModel;
});