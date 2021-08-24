using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class BooklistCardBase : ComponentBase
    {
        [Inject]
        private INavigator _nagivator { get; set; }

        [Inject]
        private IBooklistActionsService _actionsService { get; set; }

        [Inject]
        private ISeriesRepository _repository { get; set; }

        [Inject]
        private IMessenger _messenger { get; set; }

        [Parameter]
        public BookList Item { get; set; }

        [Parameter]
        public Func<BooklistAction, BookList, Task> OnItemActionCompleted { get; set; }

        public List<Comic> VisibleBooks { get; set; }

        public List<BooklistAction> Actions { get; set; } = new List<BooklistAction>();

        protected override void OnParametersSet()
        {
            UpdatePage();
        }

        private void UpdatePage()
        {
            VisibleBooks = Item.ShowHidden
                ? Item.Books.ToList()
                : Item.Books.Where(b => !b.Hidden).ToList();

            Actions = _actionsService.GetActions(Item);

            StateHasChanged();
        }

        public async Task OnBookActionCompleted(BookAction action, Comic book)
        {
            UpdatePage();
        }

        public async Task OnAction(BooklistAction action)
        {
            var success = await action.ClickAction(Item);

            if (!success)
            {
                _messenger.DisplayErrorAlert($"FAILED ACTION: {action.Caption} {Item.TypeName}");
            }
            else
            {
                UpdatePage();
                await OnItemActionCompleted(action, Item);
            }
        }
    }
}
