using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protyo.EmailSubscriptionService.Services;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.EmailSubscriptionService.Jobs.Contract;

namespace Protyo.EmailSubscriptionService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ISubscriberJob _subscriberJob;

        public Worker( ILogger<Worker> logger, ISubscriberJob subscriberJob )
        {
            _logger = logger;
            _subscriberJob = subscriberJob;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Calculate time until 9 am & 1pm to Send emails
            var now = DateTime.Now;

            var executionTime_nine_am = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
            var executionTime_one_pm = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                if (now >= executionTime_nine_am || now <= executionTime_one_pm)
                {
                    executionTime_nine_am = executionTime_nine_am.AddDays(1); // Move to next day
                    executionTime_one_pm = executionTime_one_pm.AddDays(1); // Move to next day

                    _subscriberJob.Execute();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Delay for 24 hours
            }

        }

    }
}
