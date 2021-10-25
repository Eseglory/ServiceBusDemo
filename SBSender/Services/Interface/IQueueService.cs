using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBSender.Services.Interface
{
    public interface IQueueService
    {
        Task SendMessage<T>(T serviceBusMessage, string queueName);
    }
}
