using System;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Common.Services
{
    public class BookService : IBookService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public BookService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Hide(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.Hidden = true;
                uow.Save();
            }
        }

        public void MarkAsRead(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.DateRead = DateTime.Now;
                uow.Save();
            }
        }

        public void MarkAsUnread(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.DateRead = null;
                uow.Save();
            }
        }

        public void Unhide(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Book>().GetById(id);
                comic.Hidden = false;
                uow.Save();
            }
        }
    }
}
