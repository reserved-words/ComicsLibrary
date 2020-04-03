define(['knockout'], function () {

    function ResultViewModel(params) {
        this.selected = ko.observable(false);
        this.issues = ko.observableArray([]);

        this.marvelId = params.marvelId;
        this.title = params.title;

        this.totalPages = 0;
        this.pagesFetched = 0;

        this.toggle = function (data, event) {
            var self = this;

            if (self.selected()) {
                self.selected(false);
                return;
            }

            self.selected(true);

            if (self.totalPages > 0 && (self.pagesFetched > 1))
                return;

            var url = URL.getComicsByMarvelId(self.marvelId, self.issues().length);

            AJAX.get(url, function (result) {

                self.pagesFetched = result.page;
                self.totalPages = result.totalPages;

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
        }
    }

    return ResultViewModel;
});