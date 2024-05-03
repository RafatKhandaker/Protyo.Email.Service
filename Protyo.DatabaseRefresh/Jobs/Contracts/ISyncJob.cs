
using System.Threading;

namespace Protyo.DatabaseRefresh.Jobs.Contracts
{
    public interface ISyncJob
    {
        public void Execute(CancellationToken stoppingToken);
    }
}
