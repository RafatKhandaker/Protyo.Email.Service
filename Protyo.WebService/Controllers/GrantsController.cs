using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Protyo.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrantsController : ControllerBase
    {
        private readonly ILogger<GrantsController> _logger;
        private Cache<int,GrantDataObject> MongoDBCache;
        private IMongoService<GrantDataObject> MongoService;

        public GrantsController(
                ILogger<GrantsController> logger, 
                IMongoService<GrantDataObject> mongoService, 
                ObjectExtensionHelper objectExtension,
                IConfigurationSetting configuration
            )
        {
            _logger = logger;
            MongoService = mongoService;
            MongoService.SetDatabase("ProtyoGrantListings").SetCollections("Grants");

            MongoDBCache = new Cache<int, GrantDataObject>(
                    () => objectExtension.ConvertMongoDBDocumentToDictionary(() => UpdateMongoDatabase()),
                    TimeSpan.FromMinutes(Convert.ToInt32(configuration.appSettings["DynamoSettings:RefreshTimer"]))
                );
        }

        private List<GrantDataObject> UpdateMongoDatabase() => MongoService.RetrieveAll();

        [HttpGet("All")]
        public List<GrantDataObject> GetAllGrants([FromQuery] int page = 1, [FromQuery] int size = 100) => MongoDBCache.GetAll(page, size);
        //public List<GrantDataObject> GetAllGrants() => MongoService.RetrieveAll();


        [HttpGet("All/Open")]
        public List<GrantDataObject> GetAllOpenGrants([FromQuery] int page = 1, [FromQuery] int size = 100) => MongoDBCache.GetAll(page, size).Where(w => w.CloseDate > DateTime.Now).ToList();
        // public List<GrantDataObject> GetAllOpenGrants() => MongoService.Find(Builders<GrantDataObject>.Filter.Gt("CloseDate", DateTime.Now));

        [HttpGet("{id}")]
        public GrantDataObject GetGrantById([FromRoute] int id) => MongoDBCache.Get(id);
        //public GrantDataObject GetGrantById([FromRoute] int id) => MongoService.Find(Builders<GrantDataObject>.Filter.Gt("_id", id))[0];

        [HttpGet("Funds")]
        public List<GrantDataObject> GetGrantBetweenAmountFunding([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount, [FromQuery] int page = 1, [FromQuery] int size = 100) {
            decimal awardCeiling = 0;
            decimal awardFloor = 0;

            return MongoDBCache.GetAll(page, size).Where(s => s.Details != null && s.Details.synopsis != null && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardCeiling) && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardFloor))
                                            .Where(w => awardFloor >= minAmount && awardCeiling <= maxAmount)
                                                .Where(w => w.CloseDate > DateTime.Now)
                                                    .OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.awardCeiling))
                                                            .ToList();
        }

        /*public List<GrantDataObject> GetGrantBetweenAmountFunding([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount, [FromQuery] int page = 1, [FromQuery] int size = 100)
         {
             decimal awardCeiling = 0;
             decimal awardFloor = 0;

             return MongoService.RetrieveAll().Where(s => s.Details != null && s.Details.synopsis != null && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardCeiling) && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardFloor))
                                             .Where(w => awardFloor >= minAmount && awardCeiling <= maxAmount)
                                                 .Where(w => w.CloseDate > DateTime.Now)
                                                     .OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.awardCeiling))
                                                             .ToList();
         }*/

        [HttpGet("HealthCheck")]
        public OkResult HealthCheck() => Ok();
    }
}
