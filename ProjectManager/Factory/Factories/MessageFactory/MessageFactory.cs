using ProjectManager.Models;
using ProjectManagerDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Factory.Factories.MessageFactory
{
    public class MessageFactory : IMessageFactory
    {
        public Message New(MessageViewModel viewModel)
        {
            Message message = new Message
            {
                ID = viewModel.ID,
                DeveloperID = viewModel.DeveloperID,
                Username = viewModel.Username,
                Description = viewModel.Description,
                Date = viewModel.Date
            };

            return message;
        }
    }
}