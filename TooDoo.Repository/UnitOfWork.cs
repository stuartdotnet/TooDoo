using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Model;

namespace TooDoo.Repository
{
    public class UnitOfWork : IDisposable
    {
        private ToDoContext context = new ToDoContext();
        private GenericRepository<ToDoItem> todoRepository;

        // TODO REPOSITORY
        public GenericRepository<ToDoItem> ToDoRepository
        {
            get
            {
                if (this.todoRepository == null)
                {
                    this.todoRepository = new GenericRepository<ToDoItem>(context);
                }
                return todoRepository;
            }
        }

        #region Methods
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
