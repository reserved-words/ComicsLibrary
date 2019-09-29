define(['knockout'], function (ko) {

    function ComicViewModel(params) {
        this.readUrl = params.readUrl;
        this.imageUrl = params.imageUrl;
        this.title = params.title;
        this.seriesTitle = params.seriesTitle;
        this.isRead = params.isRead;
        this.readNext = params.readNext;
        this.dateAdded = params.dateAdded;
        this.dateUpdated = params.dateUpdated;
        this.selected = params.selected;
        this.select = params.select;
        this.seriesId = params.seriesId;
        this.goToSeries = function(data,event) {
            index.loadSeries(data.seriesId);
        }
    }

    return ComicViewModel;
});