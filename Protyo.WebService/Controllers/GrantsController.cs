using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private Cache<GrantDataObject> DynamoDBCache;
        private IDynamoService DynamoService;
        public GrantsController(
                ILogger<GrantsController> logger, 
                IDynamoService dynamoService, 
                ObjectExtensionHelper objectExtension
            )
        {
            _logger = logger;
            DynamoService = dynamoService;
            DynamoService.SetTable("Grants").ScanAllAttributes();

            DynamoDBCache = new Cache<GrantDataObject>(
                    () => objectExtension.ConvertDynamoDocumentToDictionary(() => UpdateDynamoDatabase()),
                    TimeSpan.FromMinutes(5)
                );
        }

        private List<Document> UpdateDynamoDatabase() {
            var dictionaryGrantObjects = new List<Document>();
            do dictionaryGrantObjects.AddRange(DynamoService.Search.GetNextSetAsync().Result); while (!DynamoService.Search.IsDone);
            return dictionaryGrantObjects;
        } 

        [HttpGet("All")]
        public List<GrantDataObject> GetAllGrants()
        {
            return DynamoDBCache.GetAll();
        }
    }
}
