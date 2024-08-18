using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Protyo.EmailSubscriptionService.Helper;
using Protyo.EmailSubscriptionService.Services;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Protyo.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly ILogger<FormController> _logger;
        private Cache<string, FormData> GoogleSheetsCache;
        private ItemsMapper _itemsMapper;
        private GoogleSheetsHelper _googleSheetsHelper;

        private string SPREADSHEET_ID;
        private string SPREADSHEET_NAME;
        private string SHEET_VALUES;
        private int PREV_SHEET_COUNT;
        private int SHEET_COUNT;
        private string ACCESS_TOKEN;

        public FormController(
                  ILogger<FormController> logger,
                  IConfigurationSetting configuration,
                  ObjectExtensionHelper objectExtension,
                  GoogleSheetsHelper googleSheetsHelper,
                  ItemsMapper itemsMapper,
                  Cache<string, FormData> googleSheetsCache
            )
        {
            _logger = logger;
            _itemsMapper = itemsMapper;
            _googleSheetsHelper = googleSheetsHelper;

            SPREADSHEET_ID = configuration.appSettings["GoogleAppSettings:SpreadsheetId"];
            SHEET_VALUES = configuration.appSettings["GoogleAppSettings:Values"];
            SPREADSHEET_NAME = configuration.appSettings["GoogleAppSettings:SheetName"];
            ACCESS_TOKEN = configuration.appSettings["WebAccessToken"];
            SHEET_COUNT = 0;
            PREV_SHEET_COUNT = 0;

            GoogleSheetsCache = googleSheetsCache.SetInstance(
                    () => objectExtension.ConvertGoogleSheetsToDictionary(() => UpdateGoogleSheets()),
                    TimeSpan.FromMinutes(Convert.ToInt32(configuration.appSettings["GoogleAppSettings:RefreshTimer"]))
                );

        }

        private List<FormData> UpdateGoogleSheets() =>
            _itemsMapper.MapFromRangeData(
                _googleSheetsHelper.Service.Spreadsheets.Values.Get(SPREADSHEET_ID, SHEET_VALUES).Execute().Values
               );

        [HttpGet("All")]
        public List<FormData> GetAllFormData([FromHeader(Name = "access-token")] string token, [FromQuery] int page = 1, [FromQuery] int size = 100) =>
            (token.Equals(ACCESS_TOKEN)) ? GoogleSheetsCache.GetAll(page, size).ToList() : throw new Exception("Invalid Access Token!");

        [HttpDelete("Delete")]
        public string DeleteAllFormData([FromHeader(Name = "access-token")] string token){
            if (token.Equals(ACCESS_TOKEN)) 
                _googleSheetsHelper.Service.Spreadsheets.Values.Clear(new ClearValuesRequest(), SPREADSHEET_ID, SHEET_VALUES).Execute();
            else throw new Exception("Invalid Access Token!");
            return "Successfully Cleared Sheets!";
        }
       
        [HttpGet]
        public FormData GetFormDataForEmail([FromHeader(Name="access-token")] string token, [FromQuery] string email, [FromQuery] int page = 1, [FromQuery] int size = 100) =>
            (token.Equals(ACCESS_TOKEN)) ? GoogleSheetsCache.Get(email) : throw new Exception("Invalid Access Token!");

        [HttpGet("HealthCheck")]
        public OkResult HealthCheck() => Ok(); 
        
    }
}
