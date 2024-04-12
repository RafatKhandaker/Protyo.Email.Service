using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services
{
    public class DynamoService : IDynamoService
    {
        private AmazonDynamoDBClient AmazonDynamoDBClient { get; set; }

        private IConfigurationSetting _configurationSetting;

        public DynamoService(IConfigurationSetting configurationSetting) {
            _configurationSetting = configurationSetting;
            AmazonDynamoDBClient = new AmazonDynamoDBClient(
                    _configurationSetting.appSettings["DynamoSettings:AccessKeyId"], _configurationSetting.appSettings["DynamoSettings:SecretAccessKey"], Amazon.RegionEndpoint.APNortheast1
                );
        }

        public QueryResponse Query(string table, string keyConditionExpression = null, Dictionary<string, AttributeValue> expressionAttributes = null) =>
            AmazonDynamoDBClient.QueryAsync(new QueryRequest { TableName = table, KeyConditionExpression = keyConditionExpression, ExpressionAttributeValues = expressionAttributes }).GetAwaiter().GetResult();

    }
}
