using ProjectManagerDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerDataAccess.Repositories.MessageRepository
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetMessages();

        void DeleteMessage(int id);
        void Save();
    }
}
