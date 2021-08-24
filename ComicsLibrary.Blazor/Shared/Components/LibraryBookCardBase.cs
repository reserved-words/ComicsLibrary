using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Shared.Components
{
    public class LibraryBookCardBase : ComponentBase
    {
        [Inject]
        private INavigator _nagivator { get; set; }

        [Inject]
        private IBookActionsService _actionsService { get; set; }

        [Inject]
        private ISeriesRepository _repository { get; set; }

        [Inject]
        private IMessenger _messenger { get; set; }

        [Parameter]
        public Comic Book { get; set; }

        [Parameter]
        public Func<BookAction, Comic, Task> OnItemActionCompleted { get; set; }

        public bool IsNotHidden { get; set; }

        public List<BookAction> Actions { get; set; } = new List<BookAction>();

        protected override void OnParametersSet()
        {
            UpdatePage();
        }

        private void UpdatePage()
        {
            Actions = _actionsService.GetActions(Book);

            IsNotHidden = !Book.Hidden;

            StateHasChanged();
        }

        public async Task OnAction(BookAction action)
        {
            var success = await action.ClickAction(Book);

            if (!success)
            {
                _messenger.DisplayErrorAlert($"FAILED ACTION: {action.Caption} {Book.SeriesId}");
            }
            else
            {
                UpdatePage();
                await OnItemActionCompleted(action, Book);
            }
        }
    }
}
