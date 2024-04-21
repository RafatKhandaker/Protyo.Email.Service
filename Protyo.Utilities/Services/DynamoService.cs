using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services
{
    public class DynamoService : IDynamoService
    {
        public AmazonDynamoDBClient AmazonDynamoDBClient { get; set; }
        public ScanRequest ScanRequest { get; set; }
        public Table Table { get; set; }

        public Search Search { get; set; }


        private IConfigurationSetting _configurationSetting;

        public DynamoService SetScanRequest(string table, int limit) {
            ScanRequest = new ScanRequest { TableName = table, Limit = limit };
            return this;
        }

        public DynamoService SetTable(string table)
        {
            Table = Table.LoadTable(AmazonDynamoDBClient, table);
            return this;
        }

        public DynamoService(IConfigurationSetting configurationSetting) {
            _configurationSetting = configurationSetting;
            AmazonDynamoDBClient = new AmazonDynamoDBClient(
                    _configurationSetting.appSettings["DynamoSettings:AccessKeyId"], _configurationSetting.appSettings["DynamoSettings:SecretAccessKey"], Amazon.RegionEndpoint.USEast1
                );
        }

        public async Task<QueryResponse> Query(string table, string keyConditionExpression = null, Dictionary<string, AttributeValue> expressionAttributes = null) =>
            await AmazonDynamoDBClient.QueryAsync(new QueryRequest { TableName = table, KeyConditionExpression = keyConditionExpression, ExpressionAttributeValues = expressionAttributes });

        public DynamoService Scan(List<string> attributes) {
            Search = Table.Scan(new ScanOperationConfig() { AttributesToGet = attributes, Select = SelectValues.SpecificAttributes });
            return this;
        }
        public DynamoService ScanAllAttributes() { 
            Search = Table.Scan(new ScanOperationConfig() { Select = SelectValues.AllAttributes });
            return this;
        } 

        public async Task<ScanResponse> Scan() => await AmazonDynamoDBClient.ScanAsync(ScanRequest);

        public async void SaveDocument(string table, Document document) =>
            await Table.LoadTable(AmazonDynamoDBClient, table).PutItemAsync(document, new PutItemOperationConfig());
        
    }
}
