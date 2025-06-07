using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Interfaces
{
    public interface IRabbitMQPublisher
    {
        void PublishFinancialEvent(string message);
    }

}
