series = {
    id: ko.observable(),
    title: ko.observable(),
    bookLists: ko.observableArray(),
    isAbandoned: ko.observable(),
    totalIssues: ko.observable()
};

// Add methods to archive / reinstate / delete series

series.load = function (id) {
    var self = this;

    self.id(id);
    self.title("");
    self.bookLists.removeAll();
    self.isAbandoned(false);
    self.totalIssues(0);

    API.get(URL.getSeries(id), function (data) {
        self.id(data.id);
        self.title(data.title);
        self.totalIssues(data.totalComics);
        self.isAbandoned(data.abandoned);

        $(data.bookLists).each(function (index, element){
            self.addBookList(element);
        });
    });
}

series.abandonSeries = function () {
    var self = this;
    API.post(URL.abandonSeries(self.id()), null, function (result) {
        self.isAbandoned(true);
    });
}

series.reinstateSeries = function () {
    var self = this;
    API.post(URL.reinstateSeries(self.id()), null, function (result) {
        self.isAbandoned(false);
    });
}

series.deleteSeries = function () {
    if (!confirm("Delete this series?"))
        return;
    var self = this;
    API.post(URL.removeFromLibrary(self.id()), null, function (result) {
        index.loadSeries(0);
    });
}

series.addBook = function (bookList, element) {
    bookList.books.push({
        id: element.id,
        readUrl: element.readUrl,
        imageUrl: element.imageUrl,
        isRead: element.isRead,
        title: element.issueTitle,
        onSaleDate: element.onSaleDate
    });
}

series.addBookList = function (element) {
    var bookList = {
        typeId: element.typeId,
        typeName: element.typeName,
        totalBooks: element.totalBooks,
        home: ko.observable(element.home),
        books: ko.observableArray(),
    };

    bookList.home.subscribe(function (newValue) {
        var data = {
            seriesId: series.id(),
            bookTypeId: element.typeId,
            enabled: newValue
        };
        API.post(URL.setHomeOption(), data);
    });

    bookList.getMoreBooks = function (data, event) {
        var self = this;
        API.get(URL.getBooks(series.id(), bookList.typeId, bookList.books().length), function (result) {
            $(result).each(function (index, book) {
                series.addBook(bookList, book);
            });
        });
    }

    $(element.books).each(function (index, book) {
        series.addBook(bookList, book);
    });

    series.bookLists.push(bookList);
}