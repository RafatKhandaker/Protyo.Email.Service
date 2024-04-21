using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services.Contracts
{
    public interface IDynamoService
    {
        public AmazonDynamoDBClient AmazonDynamoDBClient { get; set; }
        public ScanRequest ScanRequest { get; set; }
        public Search Search { get; set; }
        public Table Table { get; set; }
        public DynamoService SetScanRequest(string table, int limit);
        public DynamoService SetTable(string table);
        public Task<QueryResponse> Query(string table, string keyConditionExpression = null, Dictionary<string, AttributeValue> expressionAttributes = null);
        public DynamoService Scan(List<string> attributes);
        public DynamoService ScanAllAttributes();
        public Task<ScanResponse> Scan();
        public void SaveDocument(string table, Document document);
    }
}
