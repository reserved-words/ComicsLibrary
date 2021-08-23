using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public class BooklistActionsService : IBooklistActionsService
    {
        private readonly INavigator _navigator;
        private readonly IMessenger _messenger;
        private readonly ISeriesRepository _repository;

        public BooklistActionsService(ISeriesRepository repository, IMessenger messenger, INavigator navigator)
        {
            _repository = repository;
            _messenger = messenger;
            _navigator = navigator;
        }

        public List<BooklistAction> GetActions(BookList booklist)
        {
            var actions = new List<BooklistAction>();

            if (booklist.Home)
            {
                actions.Add(new BooklistAction
                {
                    Caption = "Remove",
                    ClickAction = RemoveFromHome,
                    Icon = Icons.Material.Filled.Remove
                });
            }
            else
            {
                actions.Add(new BooklistAction
                {
                    Caption = "Add",
                    ClickAction = AddToHome,
                    Icon = Icons.Material.Filled.Add
                });
            }

            if (booklist.ShowHidden)
            {
                actions.Add(new BooklistAction
                {
                    Caption = "Hide Removed Books",
                    ClickAction = HideRemovedBooks,
                    Icon = Icons.Material.Filled.KeyboardHide
                });
            }
            else
            {
                actions.Add(new BooklistAction
                {
                    Caption = "Hide Removed Books",
                    ClickAction = ShowRemovedBooks,
                    Icon = Icons.Material.Filled.ShowChart
                });
            }

            return actions;
        }

        protected async Task<bool> AddToHome(BookList booklist)
        {
            _messenger.DisplaySuccessAlert("Add type to Home");
            return true;
        }

        protected async Task<bool> RemoveFromHome(BookList booklist)
        {
            _messenger.DisplaySuccessAlert("Remove type from Home");
            return true;
        }

        protected async Task<bool> ShowRemovedBooks(BookList booklist)
        {
            _messenger.DisplaySuccessAlert("Show removed books");
            return true;
        }

        protected async Task<bool> HideRemovedBooks(BookList booklist)
        {
            _messenger.DisplaySuccessAlert("Hide removed books");
            return true;
        }
    }
}
