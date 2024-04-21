using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Protyo.DatabaseRefresh.Jobs.Contracts;
using Protyo.DatabaseRefresh.Properties;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Protyo.DatabaseRefresh.Jobs
{
    public class GrantAPI_DynamoDB_SyncJob: ISyncJob
    {
        private readonly ILogger<GrantAPI_DynamoDB_SyncJob> _logger;

        private IHttpService _httpService;
        private IDynamoService _dynamoService;

        private string BaseUrl;
        public GrantAPI_DynamoDB_SyncJob(ILogger<GrantAPI_DynamoDB_SyncJob> logger, IHttpService httpService, IConfigurationSetting configurationSetting, IDynamoService dynamoService) {
            _logger = logger;
            _httpService = httpService;
            _dynamoService = dynamoService;

            BaseUrl = configurationSetting.appSettings["GrantAPIConfiguration:BaseUri"].ToString();
        }

        public async void Execute(CancellationToken stoppingToken) {
            var firstRun = true;

            _dynamoService.SetTable("Grants").Scan(new List<string>() { "GrantId" });
            var grantDocuments = new List<Document>();

            do grantDocuments.AddRange(_dynamoService.Search.GetNextSetAsync().Result); while (!_dynamoService.Search.IsDone);


            var listGrantKeys = grantDocuments.Select(s => Convert.ToInt32(s["GrantId"])).ToList();

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
                var nonExistantCount = nonExistantDBGrants.Count();

                foreach (var grant in nonExistantDBGrants)
                {
                    var document = new Document();
                    try
                    {
                        document["GrantId"] = grant.id;
                        document["AgencyCode"] = grant.agencyCode;
                        document["OppNumber"] = grant.number;
                        document["Title"] = grant.title;
                        document["Agency"] = grant.agency;
                        document["OpenDate"] = grant.openDate;
                        document["CloseDate"] = grant.closeDate;
                        document["OppStatus"] = grant.oppStatus;
                        document["DocType"] = grant.docType;
                        document["CfdaList"] = grant.cfdaList;

                        var collection = new List<KeyValuePair<string, string>> { new("oppId", Convert.ToString(grant.id)) };

                        var grantDetailsObject = JsonConvert.DeserializeObject<GrantDetails>(
                                                                _httpService.Initialize(BaseUrl + "/opportunity/details", HttpMethod.Post)
                                                                    .AddHeaders(HttpProperties.grantDetailHeaders)
                                                                    .AddContent(collection)
                                                                        .SendRequest()
                                                                            .Result.Content.ReadAsStringAsync()
                                                                                .Result);

                        document["details"] = JsonConvert.SerializeObject(grantDetailsObject);

                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation($"Exception: {e.Message}", DateTimeOffset.Now);
                        continue;
                    }

                    ExecuteRecursion(document, stoppingToken);
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
        private void ExecuteRecursion(Document document, CancellationToken stoppingToken)
        {
            try
            {
                _dynamoService.SaveDocument("Grants", document);
            }
            catch
            {
                Task.Delay(1000, stoppingToken);
                ExecuteRecursion(document, stoppingToken);
            }
        }

    }
}
