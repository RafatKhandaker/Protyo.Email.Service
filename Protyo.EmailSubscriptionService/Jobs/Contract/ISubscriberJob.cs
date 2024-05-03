using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.EmailSubscriptionService.Jobs.Contract
{
    public interface ISubscriberJob
    {
        public void Execute();
    }
}
