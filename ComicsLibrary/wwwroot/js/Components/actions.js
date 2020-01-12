define(['knockout'], function (ko) {

    function ActionsViewModel(params) {
        this.onActionCompleted = params.onActionCompleted;
        this.selectedAction = params.selectedAction;
        this.comics = params.comics;
        this.actions = [
            { url: "", name: "Select action:" },
            { url: URL.markAsRead(), name: "Mark as Read" },
            { url: URL.markAsUnread(), name: "Mark as Unread" },
            { url: URL.addToReadNext(), name: "Add to Read Next" },
            { url: URL.removeFromReadNext(), name: "Remove from Read Next" }
        ];
        this.executeAction = function () {
            var self = this;
            var selectedIds = new Array();

            $(self.comics()).each(function (index, element) {
                if (element.selected()) {
                    selectedIds.push(element.id);
                }
            });

            if (!selectedIds || selectedIds.length === 0)
                return;

            var url = self.selectedAction();
            if (!url || url.length === 0)
                return;

            AJAX.post(url, { ids: selectedIds }, function (result) {
                self.onActionCompleted();
            });
        }
    }

    return ActionsViewModel;
});