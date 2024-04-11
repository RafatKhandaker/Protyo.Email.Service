using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Protyo.EmailSubscriptionService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IEmailService _emailService;
        GoogleSheetsHelper _googleSheetsHelper;

        const string SPREADSHEET_ID = "1kJcFdID9MwRQCSSR8UYYWQAW2cOVyLeh69mirqRb4-I";
        const string SHEET_NAME = "Protyo Subscription Form (Responses)";


        public Worker(ILogger<Worker> logger, IEmailService emailService, GoogleSheetsHelper googleSheetsHelper)
        {
            _logger = logger;
            _emailService = emailService;
            _googleSheetsHelper = googleSheetsHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // Calculate time until 9 am Send emails
                var now = DateTime.Now;

                var executionTime_nine_am = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
                var executionTime_one_pm = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0);

           //     if (now >= executionTime_nine_am || now <= executionTime_one_pm)
           //     {
                    executionTime_nine_am = executionTime_nine_am.AddDays(1); // Move to next day
                    executionTime_one_pm = executionTime_one_pm.AddDays(1); // Move to next day
                    var googleSheetValues = ItemsMapper.MapFromRangeData(
                        _googleSheetsHelper.Service.Spreadsheets.Values.Get(SPREADSHEET_ID, "A:K").Execute().Values
                    );

           //     }
                var delay = executionTime_nine_am - now;

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Delay for 24 hours
            }

        }

    }
}
