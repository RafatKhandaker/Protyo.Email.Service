using System;
using System.Linq;


namespace Protyo.DatabaseRefresh.Jobs
{
    using Amazon.DynamoDBv2.DocumentModel;
    using global::Protyo.DatabaseRefresh.Jobs.Contracts;
    using global::Protyo.DatabaseRefresh.Properties;
    using global::Protyo.Utilities.Configuration.Contracts;
    using global::Protyo.Utilities.Helper;
    using global::Protyo.Utilities.Models;
    using global::Protyo.Utilities.Services.Contracts;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    namespace Protyo.DatabaseRefresh.Jobs
    {
        public class GrantAPI_MongoDB_SyncJob : ISyncJob
        {
            private readonly ILogger<GrantAPI_MongoDB_SyncJob> _logger;

            private IHttpService _httpService;
            private IMongoService<GrantDataObject> _mongoService;

            private StringCompressionHelper _compression;

            private string BaseUrl;
            public GrantAPI_MongoDB_SyncJob(
                    ILogger<GrantAPI_MongoDB_SyncJob> logger,
                    IHttpService httpService,
                    IConfigurationSetting configurationSetting,
                    IMongoService<GrantDataObject> mongoService,
                    StringCompressionHelper compression
                )
            {
                _logger = logger;
                _httpService = httpService;
                _mongoService = mongoService;
                _compression = compression;

                BaseUrl = configurationSetting.appSettings["GrantAPIConfiguration:BaseUri"].ToString();
            }

            public async void Execute(CancellationToken stoppingToken)
            {

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var firstRun = true;

                var grantDocuments = _mongoService.SetDatabase("ProtyoGrantListings").SetCollections("Grants").RetrieveAll();


                var listGrantKeys = grantDocuments.Select(s => s.GrantId).ToList();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var grantsResponseObject = JsonConvert.DeserializeObject<Grants>(
                                                                     _httpService.Initialize(BaseUrl + "/opportunities/search", HttpMethod.Post)
                                                                                     .AddHeaders(HttpProperties.grantContentHeaders)
                                                                                         .AddContent(HttpProperties.grantContent(firstRun ? null : 1))
                                                                                             .SendRequest()
                                                                                                 .Result.Content.ReadAsStringAsync()
                                                                                                     .Result);

                    var nonExistantDBGrants = grantsResponseObject.oppHits.Where(w => !listGrantKeys.Contains(w.id.Value));

                    foreach (var grant in nonExistantDBGrants)
                    {
                        var document = new GrantDataObject();
                        try
                        {
                            document.GrantId = grant.id;
                            document.AgencyCode = grant.agencyCode;
                            document.OppNumber= grant.number;
                            document.Title = grant.title;
                            document.Agency = grant.agency;

                            try{ 
                                document.OpenDate = DateTime.Parse(grant.openDate); 
                            } 
                            catch { 
                                document.OpenDate = null; 
                            }

                            try
                            {
                                document.CloseDate = DateTime.Parse(grant.closeDate);
                            }
                            catch { 
                                document.CloseDate = null; 
                            }

                            document.OppStatus = grant.oppStatus;
                            document.DocType = grant.docType;
                            document.CfdaList = grant.cfdaList ?? new List<string>();

                            var collection = new List<KeyValuePair<string, string>> { new("oppId", Convert.ToString(grant.id)) };

                            var response = _httpService.Initialize(BaseUrl + "/opportunity/details", HttpMethod.Post)
                                                                        .AddHeaders(HttpProperties.grantDetailHeaders)
                                                                        .AddContent(collection)
                                                                            .SendRequest()
                                                                                .Result.Content.ReadAsStringAsync()
                                                                                    .Result;

                            var grantDetailsObject = JsonConvert.DeserializeObject<GrantDetails>(response);

                            document.Details = grantDetailsObject;

                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation($"Exception: {e.Message}", DateTimeOffset.Now);
                            continue;
                        }

                        ExecuteRecursion(document, stoppingToken);
                    }

                    firstRun = false;

                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
            private void ExecuteRecursion(GrantDataObject document, CancellationToken stoppingToken)
            {
                try { _mongoService.InsertOne(document); } catch { Task.Delay(1000, stoppingToken); ExecuteRecursion(document, stoppingToken); }
            }

        }
    }

}
