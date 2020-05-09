series = {
    id: ko.observable(),
    title: ko.observable(),
    bookLists: ko.observableArray(),
    isAbandoned: ko.observable(),
    totalIssues: ko.observable()
};

series.hideBook = function (id, isHidden) {
    var total = series.bookLists().length;
    for (var i = 0; i < total; i++) {
        var bookList = series.bookLists()[i];
        var totalBooks = bookList.books().length;
        for (var j = 0; j < totalBooks; j++) {
            var book = bookList.books()[j];
            if (book.id === id) {
                book.hidden = isHidden;
                book.show(bookList.showHidden());
                var diff = isHidden ? 1 : -1;
                bookList.hidden(bookList.hidden() + diff);
            }
        }
    }
}

series.load = function (id) {
    if (!id || series.id() == id)
        return;

    var self = this;

    index.loading(true);

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

        index.loading(false);
    });
}

series.abandonSeries = function () {
    var self = this;
    update.archiveSeries(self.id(), function () {
        self.isAbandoned(true);
    });
}

series.reinstateSeries = function () {
    var self = this;
    update.reinstateSeries(self.id(), function () {
        self.isAbandoned(true);
    });
}

series.deleteSeries = function () {
    var self = this;
    update.deleteSeries(self.id(), function () {
        index.loadSeries(0);
    });
}

series.addBook = function (bookList, element) {
    bookList.books.push({
        id: element.id,
        seriesId: series.id(),
        readUrl: element.readUrl,
        imageUrl: element.imageUrl,
        isRead: element.isRead,
        title: element.issueTitle,
        hidden: element.hidden,
        show: ko.observable(!element.hidden)
    });

    if (element.hidden) {
        bookList.hidden(bookList.hidden() + 1);
    }
}

series.addBookList = function (element) {
    var bookList = {
        typeId: element.typeId,
        typeName: element.typeName,
        totalBooks: element.totalBooks,
        home: ko.observable(element.home),
        books: ko.observableArray(),
        hidden: ko.observable(0),
        showHidden: ko.observable(false),
        toggleHidden: function (data, event) {
            this.showHidden(!this.showHidden());
            var total = this.books().length;
            for (var j = 0; j < total; j++) {
                var book = this.books()[j];
                if (book.hidden) {
                    book.show(!book.show());
                }
            }
        }
    };

    bookList.home.subscribe(function (newValue) {
        var data = {
            seriesId: series.id(),
            bookTypeId: element.typeId,
            enabled: newValue
        };
        update.updateHomeOption(data);
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