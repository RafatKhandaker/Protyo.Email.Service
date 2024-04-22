using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protyo.EmailSubscriptionService.Services;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using Protyo.Utilities.Configuration.Contracts;

namespace Protyo.EmailSubscriptionService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IEmailService _emailService;
        GoogleSheetsHelper _googleSheetsHelper;
        ItemsMapper _itemsMapper;

        private string SPREADSHEET_ID;
        private string SHEET_NAME;
        private string SHEET_VALUES;

        public Worker(
                ILogger<Worker> logger,
                IEmailService emailService,
                GoogleSheetsHelper googleSheetsHelper,
                IConfigurationSetting configuration,
                ItemsMapper itemMapper
            )
        {
            _logger = logger;
            _emailService = emailService;
            _googleSheetsHelper = googleSheetsHelper;
            _itemsMapper = itemMapper;

            SPREADSHEET_ID = configuration.appSettings["GoogleAppSettings:SpreadsheetId"];
            SHEET_NAME = configuration.appSettings["GoogleAppSettings:SheetName"];
            SHEET_VALUES = configuration.appSettings["GoogleAppSettings:Values"];
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

                    var googleSheetValues = _itemsMapper.MapFromRangeData(
                        _googleSheetsHelper.Service.Spreadsheets.Values.Get(SPREADSHEET_ID, SHEET_VALUES).Execute().Values
                    );

                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Delay for 24 hours
            }

        }

    }
}
