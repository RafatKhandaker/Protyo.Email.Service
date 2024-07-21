using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Protyo.DatabaseRefresh.Jobs.Contracts;
using Protyo.DatabaseRefresh.Properties;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Protyo.DatabaseRefresh.Jobs
{
    public class GrantAPI_GSheetDB_SyncJob : ISyncJob
    {
        private readonly ILogger<GrantAPI_GSheetDB_SyncJob> _logger;

        private IHttpService _httpService;
        private IMongoService<UserDataObject> _mongoService;

        private string BaseUrl;
        private string DataSize;
        private string PageNumber;
        private string WebAccessToken;

        public GrantAPI_GSheetDB_SyncJob(
                    ILogger<GrantAPI_GSheetDB_SyncJob> logger,
                    IHttpService httpService,
                    IConfigurationSetting configurationSetting,
                    IMongoService<UserDataObject> mongoService,
                    StringCompressionHelper compression
                )
        {
            _logger = logger;
            _httpService = httpService;
            _mongoService = mongoService;

            BaseUrl = configurationSetting.appSettings["ApiEndpointConfiguration:BaseUri"].ToString();
            DataSize = configurationSetting.appSettings["ApiEndpointConfiguration:DataSize"].ToString();
            PageNumber = configurationSetting.appSettings["ApiEndpointConfiguration:PageNumber"].ToString();
            WebAccessToken = configurationSetting.appSettings["WebAccessToken"].ToString();

            _mongoService.SetDatabase("ProtyoGrantListings").SetCollections("Users");
        }

        public async void Execute(CancellationToken stoppingToken)
        {
            var formDataResonse = JsonConvert.DeserializeObject<List<FormData>>(
                                                _httpService.Initialize(BaseUrl + "All" + "?page=" + PageNumber + "&size=" + DataSize, HttpMethod.Get)
                                                                .AddHeaders(HttpProperties.formGetHeaders(WebAccessToken))
                                                                    .SendRequest()
                                                                        .Result.Content.ReadAsStringAsync().Result );
            formDataResonse.ForEach(form =>{
                    try { 
                        _mongoService.Update( Builders<UserDataObject>.Filter.Eq(p => p.email, form.email),
                                Builders<UserDataObject>.Update.Set(p => p.name, form.yourName)
                                                               .Set(p => p.email, form.email)
                                                               .Set(p => p.phone, form.phoneNumber)
                                                               .Set(p => p.address, form.address)
                                                               .Set(p => p.formInput, form) );  

                    } catch(Exception ex) { _logger.LogError(ex.Message); }
              });

            var response = _httpService.Initialize(BaseUrl + "Delete", HttpMethod.Delete)
                                        .AddHeaders(HttpProperties.formGetHeaders(WebAccessToken))
                                            .SendRequest().Result.Content.ReadAsStringAsync();
        }
    }
}
