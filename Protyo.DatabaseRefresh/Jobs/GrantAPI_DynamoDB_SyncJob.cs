using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Protyo.DatabaseRefresh.Jobs.Contracts;
using Protyo.DatabaseRefresh.Properties;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Helper;
using Protyo.Utilities.Models;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Protyo.DatabaseRefresh.Jobs
{
    public class GrantAPI_DynamoDB_SyncJob: ISyncJob
    {
        private readonly ILogger<GrantAPI_DynamoDB_SyncJob> _logger;

        private IHttpService _httpService;
        private IDynamoService _dynamoService;

        private StringCompressionHelper _compression;

        private string BaseUrl;
        public GrantAPI_DynamoDB_SyncJob(
                ILogger<GrantAPI_DynamoDB_SyncJob> logger,
                IHttpService httpService,
                IConfigurationSetting configurationSetting,
                IDynamoService dynamoService,
                StringCompressionHelper compression
            ) {
            _logger = logger;
            _httpService = httpService;
            _dynamoService = dynamoService;
            _compression = compression;

            BaseUrl = configurationSetting.appSettings["GrantAPIConfiguration:BaseUri"].ToString();
        }

        public async void Execute(CancellationToken stoppingToken) {

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

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
                        document["CfdaList"] = grant.cfdaList?? new List<string>();

                        var collection = new List<KeyValuePair<string, string>> { new("oppId", Convert.ToString(grant.id)) };

                        var grantDetailsObject = JsonConvert.DeserializeObject<GrantDetails>(
                                                                _httpService.Initialize(BaseUrl + "/opportunity/details", HttpMethod.Post)
                                                                    .AddHeaders(HttpProperties.grantDetailHeaders)
                                                                    .AddContent(collection)
                                                                        .SendRequest()
                                                                            .Result.Content.ReadAsStringAsync()
                                                                                .Result);

                        var details = JsonConvert.SerializeObject(grantDetailsObject); 
                        var detailSize = Encoding.UTF8.GetByteCount(details);
                        document["details"] = (detailSize >= 400000)? _compression.CompressString(details) : details;


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
        private void ExecuteRecursion(Document document, CancellationToken stoppingToken)
        {
            try{ _dynamoService.SaveDocument("Grants", document); }catch{ Task.Delay(1000, stoppingToken); ExecuteRecursion(document, stoppingToken); }
        }

    }
}
