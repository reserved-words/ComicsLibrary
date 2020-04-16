define(['knockout'], function () {

    function ResultViewModel(params) {
        this.selected = ko.observable(false);
        this.issues = ko.observableArray([]);
        this.totalPages = ko.observable(0);
        this.pagesFetched = ko.observable(0);
        this.libraryId = ko.observable(params.libraryId);

        this.sourceId = params.sourceId;
        this.sourceItemId = params.sourceItemId;
        this.title = params.title;
        this.url = params.url;
        this.imageUrl = params.imageUrl;

        this.getMoreIssues = function (data, event) {
            var self = this;

            var url = URL.getComicsBysourceItemId(self.sourceItemId, self.issues().length);

            API.get(url, function (result) {
                self.pagesFetched(result.page);
                self.totalPages(result.totalPages);

                $(result.results).each(function (index, element) {

                    self.issues.push({
                        id: element.id,
                        title: element.issueTitle,
                        imageUrl: element.imageUrl,
                        readUrl: element.readUrl,
                        isRead: element.isRead,
                        onSaleDate: element.onSaleDate
                    });

                });

            });
        };

        this.toggle = function (data, event) {

            if (this.selected()) {
                this.selected(false);
                return;
            }

            this.selected(true);

            if (this.totalPages() > 0 && this.pagesFetched() >= 1)
                return;

            this.getMoreIssues();
        }

        this.goToSeries = function () {
            index.loadSeries(this.libraryId());
        }

        this.addToLibrary = function () {
            var data = {
                sourceId: this.sourceId,
                sourceItemId: this.sourceItemId,
                title: this.title,
                imageUrl: this.imageUrl,
                url: this.url
            };

            var self = this;

            API.post(URL.addToLibrary(), data, function (result) {
                
                if (!result) {
                    alert("Error");
                    return;
                }
                self.libraryId(result);
            });
        }
    }

    return ResultViewModel;
});