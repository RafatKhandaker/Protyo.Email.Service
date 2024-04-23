using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protyo.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrantsController : ControllerBase
    {
        private readonly ILogger<GrantsController> _logger;
        private Cache<int,GrantDataObject> DynamoDBCache;
        private IDynamoService DynamoService;

        public GrantsController(
                ILogger<GrantsController> logger, 
                IDynamoService dynamoService, 
                ObjectExtensionHelper objectExtension,
                IConfigurationSetting configuration
            )
        {
            _logger = logger;
            DynamoService = dynamoService;
            DynamoService.SetTable("Grants").ScanAllAttributes();

            DynamoDBCache = new Cache<int,GrantDataObject>(
                    () => objectExtension.ConvertDynamoDocumentToDictionary(() => UpdateDynamoDatabase()),
                    TimeSpan.FromMinutes(Convert.ToInt32(configuration.appSettings["DynamoSettings:RefreshTimer"]))
                );
        }

        private List<Document> UpdateDynamoDatabase() {
            var dictionaryGrantObjects = new List<Document>();
            do ExecuteRecursion(ref dictionaryGrantObjects); while (!DynamoService.Search.IsDone);
            return dictionaryGrantObjects;
        }

        private void ExecuteRecursion(ref List<Document> grantObjects )
        {
            try { 
                grantObjects.AddRange(DynamoService.Search.GetNextSetAsync().Result); 
            } 
            catch(Exception e) {
                _logger.LogWarning(e.Message);
                Task.Delay(1000);
                ExecuteRecursion(ref grantObjects);
             }
        }

        [HttpGet("All")]
        public List<GrantDataObject> GetAllGrants() => DynamoDBCache.GetAll();

        [HttpGet("All/Open")]
        public List<GrantDataObject> GetAllOpenGrants() => DynamoDBCache.GetAll().Where(w => w.CloseDate > DateTime.Now).ToList();

        [HttpGet("{id}")]
        public GrantDataObject GetGrantById([FromRoute] int id) => DynamoDBCache.Get(id);

        [HttpGet("Funds")]
        public List<GrantDataObject> GetGrantBetweenAmountFunding([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount) {
            decimal awardCeiling = 0;
            decimal awardFloor = 0;

            return DynamoDBCache.GetAll().Where(s => s.Details != null && s.Details.synopsis != null && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardCeiling) && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardFloor))
                                            .Where(w => awardFloor >= minAmount && awardCeiling <= maxAmount)
                                                .Where(w => w.CloseDate > DateTime.Now)
                                                    .OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.awardCeiling))
                                                            .ToList();
        }
        

        [HttpGet("HealthCheck")]
        public OkResult HealthCheck() => Ok();
    }
}
