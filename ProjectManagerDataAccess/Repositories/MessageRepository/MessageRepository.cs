using ProjectManagerDB;
using ProjectManagerDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerDataAccess.Repositories.MessageRepository
{
    public class MessageRepository : IMessageRepository , IDisposable
    {
        private readonly ProjectManagerDbContext Context;
        private bool disposed = false;

        public MessageRepository(ProjectManagerDbContext context)
        {
            this.Context = context;
        }

        public void New(Message message)
        {
            Context.Messages.Add(message);
        }

        public IEnumerable<Message> GetMessages()
        {
            return Context.Messages.Take(20).OrderByDescending(i => i.ID).ToList();
        }

        

        public void DeleteMessage(int id)
        {
            Message message = Context.Messages.Find(id);
            Context.Messages.Remove(message);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
