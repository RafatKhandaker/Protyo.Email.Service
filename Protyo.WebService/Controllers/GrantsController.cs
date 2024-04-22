using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;

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
            do dictionaryGrantObjects.AddRange(DynamoService.Search.GetNextSetAsync().Result); while (!DynamoService.Search.IsDone);
            return dictionaryGrantObjects;
        } 

        [HttpGet("All")]
        public List<GrantDataObject> GetAllGrants() => DynamoDBCache.GetAll();
        

        [HttpGet("{id}")]
        public GrantDataObject GetGrantById([FromRoute] int id) => DynamoDBCache.Get(id);
        

        [HttpGet("HealthCheck")]
        public HttpResponse HealthCheck()
        {
            Response.StatusCode = 200;
            return Response;
        }
    }
}
