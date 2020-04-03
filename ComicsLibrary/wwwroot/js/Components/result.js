﻿define(['knockout'], function () {

    function ResultViewModel(params) {
        this.selected = ko.observable(false);
        this.issues = ko.observableArray([]);
        this.totalPages = ko.observable(0);
        this.pagesFetched = ko.observable(0);
        this.libraryId = ko.observable(params.libraryId);

        this.marvelId = params.marvelId;
        this.title = params.title;
        this.imageUrl = params.imageUrl;
        this.startYear = params.startYear;
        this.endYear = params.endYear;
        this.type = params.type;
        this.url = params.url;

        this.getMoreIssues = function (data, event) {
            var self = this;

            var url = URL.getComicsByMarvelId(self.marvelId, self.issues().length);

            AJAX.get(url, function (result) {
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
                marvelId: this.marvelId,
                title: this.title,
                imageUrl: this.imageUrl,
                startYear: this.startYear,
                endYear: this.endYear,
                type: this.type,
                url: this.url
            };

            var self = this;

            AJAX.post(URL.addToLibrary(), data, function (result) {
                alert(result);

                if (result === 0) {
                    alert("Error");
                    return;
                }
                self.libraryId(result);
            });
        }
    }

    return ResultViewModel;
});