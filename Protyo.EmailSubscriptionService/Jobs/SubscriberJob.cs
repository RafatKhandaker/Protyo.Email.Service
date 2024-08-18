using Newtonsoft.Json;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.EmailSubscriptionService.Jobs.Contract;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Protyo.EmailSubscriptionService.Jobs
{
    public class SubscriberJob : ISubscriberJob
    {
        private IEmailService _emailService;
        GoogleSheetsHelper _googleSheetsHelper;
        ItemsMapper _itemsMapper;

        private string SPREADSHEET_ID;
        private string SHEET_NAME;
        private string SHEET_VALUES;

        private string PathToHtml;
        public SubscriberJob(
                IEmailService emailService,
                GoogleSheetsHelper googleSheetsHelper,
                IConfigurationSetting configuration,
                ItemsMapper itemMapper
            ) {
            _emailService = emailService;
            _googleSheetsHelper = googleSheetsHelper;
            _itemsMapper = itemMapper;

            SPREADSHEET_ID = configuration.appSettings["GoogleAppSettings:SpreadsheetId"];
            SHEET_NAME = configuration.appSettings["GoogleAppSettings:SheetName"];
            SHEET_VALUES = configuration.appSettings["GoogleAppSettings:Values"];

            PathToHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Properties/EmailTemplate.html");
        }

        public async void Execute() {
            var googleSheetValues = _itemsMapper.MapFromRangeData(
                   _googleSheetsHelper.Service.Spreadsheets.Values.Get(SPREADSHEET_ID, SHEET_VALUES).Execute().Values
               );

            var grantIds = new int[] { 353888, 353909, 353907, 353906, 353908 };
            var grantList = new List<GrantDataObject>();

            /* using (HttpClient client = new HttpClient())
             {
                 foreach (var grant in grantIds) 
                 {
                     try
                     {
                         var response = await client.GetAsync($"http://ec2-18-207-218-255.compute-1.amazonaws.com/Grants/{grant}");

                         if (response.IsSuccessStatusCode)
                         {
                             var responseBody = await response.Content.ReadAsStringAsync();
                             var grantObject = JsonConvert.DeserializeObject<GrantDataObject>(responseBody);

                             grantList.Add(grantObject);
                         }
                     }
                     catch (Exception ex) { continue; }
                 }
             }*/

            //  _emailService.emailListing = googleSheetValues.Select(s => s.email).Skip(1).ToList();
            _emailService.emailListing = new string[] { "rafat.khandaker@gmail.com", "aleef@protyo.com", "mbarnett@protyo.com", "dbennett@protyo.com", "epetersen@protyo.com" };


            _emailService.sendHtmlFromFilePath("Test Grant System", PathToHtml);

        }
    }
}
