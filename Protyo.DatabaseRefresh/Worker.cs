
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protyo.DatabaseRefresh.Jobs.Contracts;
using Protyo.DatabaseRefresh.Jobs.Protyo.DatabaseRefresh.Jobs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Protyo.DatabaseRefresh
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private List<ISyncJob> _jobs;
        public Worker(ILogger<Worker> logger, GrantAPI_MongoDB_SyncJob grantAPI_MongoDB_SyncJob)
        {
            _logger = logger;
            _jobs = new List<ISyncJob>();
            _jobs.Add(grantAPI_MongoDB_SyncJob);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) => Run(stoppingToken);

        private void Run(CancellationToken stoppingToken) {
            try { foreach (var j in _jobs) j.Execute(stoppingToken); } catch (Exception e) { _logger.LogError(e.Message); }
        }
    }
}
