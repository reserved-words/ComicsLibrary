using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Blazor.Services;
using ComicsLibrary.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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
        private Services.ISeriesRepository _repository { get; set; }

        [Inject]
        private IMessenger _messenger { get; set; }

        [Parameter]
        public BookList Item { get; set; }

        public List<BooklistAction> Actions { get; set; } = new List<BooklistAction>();

        protected override void OnParametersSet()
        {
            Actions = _actionsService.GetActions(Item);

            StateHasChanged();
        }

        public async Task OnAction(BooklistAction action)
        {
            var success = await action.ClickAction(Item);

            // Might need to update some stuff

            if (!success)
            {
                _messenger.DisplayErrorAlert($"FAILED ACTION: {action.Caption} {Item.TypeName}");
            }
        }
    }
}
