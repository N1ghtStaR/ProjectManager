using ProjectManager.Models;
using ProjectManagerDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Factory.Factories.MessageFactory
{
    interface IMessageFactory
    {
        Message New(MessageViewModel viewModel);
    }
}
