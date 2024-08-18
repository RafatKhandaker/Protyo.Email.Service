using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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
        private ListCache<GrantDataObject> MongoDBCache;
        private IMongoService<GrantDataObject> MongoService;

        public GrantsController(
                ILogger<GrantsController> logger,
                IMongoService<GrantDataObject> mongoService,
                ObjectExtensionHelper objectExtension,
                IConfigurationSetting configuration,
                ListCache<GrantDataObject> mongoDBCache
            )
        {
            _logger = logger;
            MongoService = mongoService;
            MongoService.SetDatabase("ProtyoGrantListings").SetCollections("Grants");

            MongoDBCache = mongoDBCache.SetInstance(
                        () => UpdateMongoDatabase(),
                        TimeSpan.FromMinutes(Convert.ToInt32(configuration.appSettings["Database:RefreshTimer"]))
                    );
        }

        private List<GrantDataObject> UpdateMongoDatabase() => MongoService.RetrieveAll();

        [HttpGet("All")]
        public List<GrantDataObject> GetAllGrants() => MongoDBCache.CacheStorage;
        //public List<GrantDataObject> GetAllGrants() => MongoService.RetrieveAll();

        [HttpGet("All/Open")]
        public List<GrantDataObject> GetAllOpenGrants() => MongoDBCache.CacheStorage.Where(w => w.CloseDate > DateTime.Now).ToList();
        // public List<GrantDataObject> GetAllOpenGrants() => MongoService.Find(Builders<GrantDataObject>.Filter.Gt("CloseDate", DateTime.Now));

        [HttpGet("{id}")]
        public GrantDataObject GetGrantById([FromRoute] int id) => MongoDBCache.Get(id);
        //public GrantDataObject GetGrantById([FromRoute] int id) => MongoService.Find(Builders<GrantDataObject>.Filter.Gt("_id", id))[0];

        [HttpGet("Funds")]
        public List<GrantDataObject> GetGrantBetweenAmountFunding([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount, [FromQuery] int page = 1, [FromQuery] int size = 100) {
            decimal awardCeiling = 0;
            decimal awardFloor = 0;

            return MongoDBCache.GetAll(page, size).Where(s => s.Details != null && s.Details.synopsis != null && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardCeiling) && decimal.TryParse(s.Details.synopsis.awardFloor, out awardFloor))
                                            .Where(w => awardFloor >= minAmount && awardCeiling <= maxAmount)
                                                .Where(w => w.CloseDate > DateTime.Now)
                                                    .OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.awardCeiling))
                                                            .ToList();
        }


        [HttpGet("Json/All")]
        public string GetAllGrantsJson() => MongoDBCache.CacheStorage.ToJson();

        [HttpGet("Json/All/Open")]
        public string GetAllOpenGrantsJson() => MongoDBCache.CacheStorage.Where(w => w.CloseDate > DateTime.Now).ToJson();

        [HttpGet("Json/{id}")]
        public string GetGrantByIdJson([FromRoute] int id) => MongoDBCache.Get(id).ToJson();

        [HttpGet("Json/Funds")]
        public string GetGrantBetweenAmountFundingJson([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount)
        {
            decimal awardCeiling = 0;
            decimal awardFloor = 0;

            return MongoDBCache.CacheStorage.Where(s => s.Details != null && s.Details.synopsis != null && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardCeiling) && decimal.TryParse(s.Details.synopsis.awardFloor, out awardFloor))
                                            .Where(w => awardFloor >= minAmount && awardCeiling <= maxAmount)
                                                .Where(w => w.CloseDate > DateTime.Now)
                                                    .OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.awardCeiling))
                                                            .ToJson();
        }

        [HttpGet("Query/All")]
        public List<GrantDataObject> GetAllGrants([FromQuery] int page = 1, [FromQuery] int size = 100) => MongoDBCache.GetAll(page, size);

        [HttpGet("Query/All/Open")]
        public List<GrantDataObject> GetAllOpenGrants([FromQuery] int page = 1, [FromQuery] int size = 100) => MongoDBCache.GetAll(page, size).Where(w => w.CloseDate > DateTime.Now).ToList();
        // public List<GrantDataObject> GetAllOpenGrants() => MongoService.Find(Builders<GrantDataObject>.Filter.Gt("CloseDate", DateTime.Now));
        [HttpGet("Query/Json/Funds")]
        public string GetGrantBetweenAmountFundingJson([FromQuery] decimal minAmount, [FromQuery] decimal maxAmount, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            decimal awardCeiling = 0;
            decimal awardFloor = 0;

            return MongoDBCache.GetAll(page, size).Where(s => s.Details != null && s.Details.synopsis != null && decimal.TryParse(s.Details.synopsis.awardCeiling, out awardCeiling) && decimal.TryParse(s.Details.synopsis.awardFloor, out awardFloor))
                                            .Where(w => awardFloor >= minAmount && awardCeiling <= maxAmount)
                                                .Where(w => w.CloseDate > DateTime.Now)
                                                    .OrderByDescending(o => Convert.ToDecimal(o.Details.synopsis.awardCeiling))
                                                            .ToJson();
        }


        [HttpGet("HealthCheck")]
        public OkResult HealthCheck() => Ok();
    }
}
