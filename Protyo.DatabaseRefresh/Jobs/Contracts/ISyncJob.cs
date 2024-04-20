using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Protyo.DatabaseRefresh.Jobs.Contracts
{
    public interface ISyncJob
    {
        public void Execute(CancellationToken stoppingToken);
    }
}
