﻿define(['knockout'], function (ko) {

    function ComicViewModel(params) {

        this.id = params.id;
        this.seriesTitle = params.seriesTitle;
        this.issueTitle = params.issueTitle;
        this.imageUrl = params.imageUrl;
        this.readUrl = params.readUrl;
        this.seriesId = params.seriesId;
        this.onSaleDate = params.onSaleDate;
        this.unreadIssues = params.unreadIssues;

        this.goToSeries = function(data,event) {
            index.loadSeries(data.seriesId);
        }

        this.markAsRead = function (data, event) {
            homeViewModel.markAsRead(data.id);
        }

        this.archiveSeries = function (data, event) {
            homeViewModel.archiveSeries(data.seriesId);
        }
    }

    return ComicViewModel;
});