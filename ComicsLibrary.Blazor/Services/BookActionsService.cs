using ComicsLibrary.Blazor.Model;
using ComicsLibrary.Common;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public class BookActionsService : IBookActionsService
    {
        private readonly INavigator _navigator;
        private readonly IMessenger _messenger;
        private readonly ISeriesRepository _repository;

        public BookActionsService(ISeriesRepository repository, IMessenger messenger, INavigator navigator)
        {
            _repository = repository;
            _messenger = messenger;
            _navigator = navigator;
        }
        
        public List<BookAction> GetActions(Comic book)
        {
            var actions = new List<BookAction>();

            if (book.IsRead)
            {
                actions.Add(new BookAction
                {
                    Caption = "Mark As Unread",
                    ClickAction = MarkAsUnread,
                    Icon = Icons.Material.Filled.MarkAsUnread
                });
            }
            else
            {
                actions.Add(new BookAction
                {
                    Caption = "Test",
                    ClickAction = MarkAsRead,
                    Icon = Icons.Material.Filled.MarkChatRead
                });
            }

            if (book.Hidden)
            {
                actions.Add(new BookAction
                {
                    Caption = "Unhide",
                    ClickAction = Unhide,
                    Icon = Icons.Material.Filled.Upgrade
                });
            }
            else
            {
                actions.Add(new BookAction
                {
                    Caption = "Hide",
                    ClickAction = Hide,
                    Icon = Icons.Material.Filled.CloudDownload
                });
            }

            return actions;
        }

        protected async Task<bool> MarkAsUnread(Comic book)
        {
            _messenger.DisplaySuccessAlert("Mark as unread");
            book.IsRead = false;
            book.DateRead = null;
            return true;
        }

        protected async Task<bool> MarkAsRead(Comic book)
        {
            _messenger.DisplaySuccessAlert("Mark as read");
            book.IsRead = true;
            book.DateRead = DateTime.Now;
            return true;
        }

        protected async Task<bool> Hide(Comic book)
        {
            _messenger.DisplaySuccessAlert("Hide");
            book.Hidden = true;
            return true;
        }

        protected async Task<bool> Unhide(Comic book)
        {
            _messenger.DisplaySuccessAlert("Unhide");
            book.Hidden = false;
            return true;
        }
    }
}
