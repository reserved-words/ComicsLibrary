using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor
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
                    Caption = "Remove from Home Types",
                    ClickAction = RemoveFromHome,
                    Icon = Icons.Material.Filled.Remove
                });
            }
            else
            {
                actions.Add(new BooklistAction
                {
                    Caption = "Add to Home Types",
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
                    Caption = "Show Removed Books",
                    ClickAction = ShowRemovedBooks,
                    Icon = Icons.Material.Filled.ShowChart
                });
            }

            return actions;
        }

        protected async Task<bool> AddToHome(BookList booklist)
        {
            booklist.Home = true;
            // Save changes - to do
            _messenger.DisplaySuccessAlert($"Add {booklist.TypeName} to Home Types");
            return true;
        }

        protected async Task<bool> RemoveFromHome(BookList booklist)
        {
            booklist.Home = false;
            // Save changes - to do
            _messenger.DisplaySuccessAlert($"Remove {booklist.TypeName} from Home Types");
            return true;
        }

        protected async Task<bool> ShowRemovedBooks(BookList booklist)
        {
            booklist.ShowHidden = true;
            _messenger.DisplaySuccessAlert($"Show hidden {booklist.TypeName}");
            return true;
        }

        protected async Task<bool> HideRemovedBooks(BookList booklist)
        {
            booklist.ShowHidden = false;
            _messenger.DisplaySuccessAlert($"Hide hidden {booklist.TypeName}");
            return true;
        }
    }
}
