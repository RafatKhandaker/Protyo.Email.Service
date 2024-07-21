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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private Cache<long, UserDataObject> MongoDBCache;
        private IMongoService<UserDataObject> MongoService;

        private string ACCESS_TOKEN;

        public UsersController(
                ILogger<UsersController> logger,
                IMongoService<UserDataObject> mongoService,
                ObjectExtensionHelper objectExtension,
                IConfigurationSetting configuration
            )
        {
            _logger = logger;
            MongoService = mongoService;
            MongoService.SetDatabase("ProtyoGrantListings").SetCollections("Users");

            ACCESS_TOKEN = configuration.appSettings["WebAccessToken"];

            MongoDBCache = new Cache<long, UserDataObject>(
                    () => objectExtension.ConvertMongoDBDocumentToDictionary(() => UpdateMongoDatabase()),
                    TimeSpan.FromMinutes(Convert.ToInt32(configuration.appSettings["Database:RefreshTimer"]))
                );
        }

        private List<UserDataObject> UpdateMongoDatabase() => MongoService.RetrieveAll();

        [HttpGet("All")]
        public List<UserDataObject> GetAllUsers([FromHeader(Name = "access-token")] string token, [FromQuery] int page = 1, [FromQuery] int size = 100) =>
            (token.Equals(ACCESS_TOKEN)) ? MongoDBCache.GetAll(page, size) : throw new Exception("Invalid Access Token!");

    }
}
