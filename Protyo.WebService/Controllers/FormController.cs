using Microsoft.AspNetCore.Http;
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
using System.Threading.Tasks;

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
        private string SHEET_VALUES;
        private int PREV_SHEET_COUNT;
        private int SHEET_COUNT;
        private string ACCESS_TOKEN;

        public FormController(
                  ILogger<FormController> logger,
                  IConfigurationSetting configuration,
                  ObjectExtensionHelper objectExtension,
                  GoogleSheetsHelper googleSheetsHelper,
                  ItemsMapper itemsMapper
            )
        {
            _logger = logger;
            _itemsMapper = itemsMapper;
            _googleSheetsHelper = googleSheetsHelper;

            SPREADSHEET_ID = configuration.appSettings["GoogleAppSettings:SpreadsheetId"];
            SHEET_VALUES = configuration.appSettings["GoogleAppSettings:Values"];
            ACCESS_TOKEN = configuration.appSettings["WebAccessToken"];

            SHEET_COUNT = 0;
            PREV_SHEET_COUNT = 0;

            GoogleSheetsCache = new Cache<string, FormData>(
                    () => objectExtension.ConvertGoogleSheetsToDictionary(() => UpdateGoogleSheets()),
                    TimeSpan.FromMinutes(Convert.ToInt32(configuration.appSettings["GoogleAppSettings:RefreshTimer"]))
                );
        }

        private List<FormData> UpdateGoogleSheets()
        {
           var sheetData = _itemsMapper.MapFromRangeData(
                            _googleSheetsHelper.Service.Spreadsheets.Values.Get(SPREADSHEET_ID, 
                            SHEET_VALUES.Replace(Convert.ToString(PREV_SHEET_COUNT), Convert.ToString(SHEET_COUNT))
                        ).Execute().Values
                    );
            PREV_SHEET_COUNT = SHEET_COUNT;
            SHEET_COUNT = sheetData.Count();
            return sheetData;
        }
        

        [HttpGet("All")]
        public List<FormData> GetAllFormData([FromHeader(Name = "access-token")] string token) =>
            (token.Equals(ACCESS_TOKEN)) ? GoogleSheetsCache.GetAll() : throw new Exception("Invalid Access Token!");

        [HttpGet]
        public FormData GetFormDataForEmail([FromHeader(Name="access-token")] string token, [FromQuery] string email) =>
            (token.Equals(ACCESS_TOKEN))? GoogleSheetsCache.Get(email) : throw new Exception("Invalid Access Token!");

        [HttpGet("HealthCheck")]
        public HttpResponse HealthCheck()
        {
            Response.StatusCode = 200;
            return Response;
        }
    }
}
