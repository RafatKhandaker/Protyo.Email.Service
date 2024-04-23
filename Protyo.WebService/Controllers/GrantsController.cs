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
            catch {
                Task.Delay(1000);
                ExecuteRecursion(ref grantObjects);
             }
        }

        [HttpGet("All")]
        public List<GrantDataObject> GetAllGrants() => DynamoDBCache.GetAll();
        

        [HttpGet("{id}")]
        public GrantDataObject GetGrantById([FromRoute] int id) => DynamoDBCache.Get(id);

        [HttpGet("Funds")]
        public List<GrantDataObject> GetGrantByMaxAmountFunding([FromQuery] decimal maxAmount) => 
            DynamoDBCache.GetAll().Where(w => Convert.ToDecimal(w.Details.synopsis.estimatedFunding) > maxAmount ).OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.estimatedFunding)).ToList();

        [HttpGet("HealthCheck")]
        public OkResult HealthCheck() => Ok();
    }
}
