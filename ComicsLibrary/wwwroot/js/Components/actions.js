define(['knockout'], function (ko) {

    function ActionsViewModel(params) {
        this.onActionCompleted = params.onActionCompleted;
        this.selectedAction = params.selectedAction;
        this.comics = params.comics;
        this.actions = [
            { url: "", name: "Select action:" },
            { url: URL.get("markAsRead"), name: "Mark as Read" },
            { url: URL.get("markAsUnread"), name: "Mark as Unread" },
            { url: URL.get("addToReadNext"), name: "Add to Read Next" },
            { url: URL.get("removeFromReadNext"), name: "Remove from Read Next" }
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