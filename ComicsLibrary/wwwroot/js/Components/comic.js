define(['knockout'], function (ko) {

    function ComicViewModel(params) {
        this.readUrl = params.readUrl;
        this.imageUrl = params.imageUrl;
        this.title = params.title;
        this.seriesTitle = params.seriesTitle;
        this.seriesId = params.seriesId;
        this.goToSeries = function(data,event) {
            index.loadSeries(data.seriesId);
        }
    }

    return ComicViewModel;
});