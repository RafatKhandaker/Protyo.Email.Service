using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using Protyo.Utilities.Models;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.IO.Compression;
using MongoDB.Bson;

namespace Protyo.Utilities.Helper
{
    public class ObjectExtensionHelper
    {
        public Dictionary<int, GrantDataObject> ConvertDynamoDocumentToDictionary(Func<List<Document>> dynamoDocumentRetriever) 
        {
            var dictionaryGrantObjects = new Dictionary<int, GrantDataObject>();
            foreach (var document in dynamoDocumentRetriever()) {
                var grantdataObject = new GrantDataObject();

                if(document.ContainsKey("GrantId")) 
                    grantdataObject.GrantId = Convert.ToInt32(document["GrantId"]);
                if (document.ContainsKey("AgencyCode")) 
                    grantdataObject.AgencyCode = document["AgencyCode"].AsString();
                if (document.ContainsKey("OppNumber"))
                    grantdataObject.OppNumber = document["OppNumber"].AsString();
                if (document.ContainsKey("Title"))
                    grantdataObject.Title = document["Title"].AsString();
                if (document.ContainsKey("Agency"))
                    grantdataObject.Agency = document["Agency"].AsString();
                if (document.ContainsKey("OpenDate"))
                    grantdataObject.OpenDate = document["OpenDate"].AsDateTime();
                if (document.ContainsKey("CloseDate"))
                    grantdataObject.CloseDate = document["CloseDate"].AsDateTime();
                if (document.ContainsKey("DocType"))
                    grantdataObject.DocType = document["DocType"].AsString();
                if (document.ContainsKey("CfdaList"))
                    grantdataObject.CfdaList = document["CfdaList"].AsListOfString();
                if (document.ContainsKey("details")) {
                    try { 
                        grantdataObject.Details = JsonConvert.DeserializeObject<GrantDetails>(document["details"].AsString()); 
                    } 
                    catch { 
                        grantdataObject.Details = JsonConvert.DeserializeObject<GrantDetails>(DecompressBytes(document["details"].AsByteArray())); 
                    }
                }
                    
                dictionaryGrantObjects.Add(grantdataObject.GrantId.Value, grantdataObject);
            }

            return dictionaryGrantObjects;

        }

        public Dictionary<int, GrantDataObject> ConvertMongoDBDocumentToDictionary(Func<List<GrantDataObject>> mongoDocumentRetriever)
        {
            var dictionaryGrantObjects = new Dictionary<int, GrantDataObject>();
            foreach (var document in mongoDocumentRetriever())
                dictionaryGrantObjects.Add(document.GrantId.Value, document);
            
            return dictionaryGrantObjects;
        }

        public Dictionary<long, UserDataObject> ConvertMongoDBDocumentToDictionary(Func<List<UserDataObject>> mongoDocumentRetriever)
        {
            var dictionaryUserObjects = new Dictionary<long, UserDataObject>();
            foreach (var document in mongoDocumentRetriever())
                dictionaryUserObjects.Add( BitConverter.ToInt64(document._Id.ToByteArray()) , document);

            return dictionaryUserObjects;
        }

        public Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>( Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> merger) => dictionary.Concat(merger).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        public Dictionary<string, FormData> ConvertGoogleSheetsToDictionary(Func<List<FormData>> googleSheetRetriever)
        {
            var dictionaryFormObjects = new Dictionary<string, FormData>();
            foreach (var data in googleSheetRetriever()) 
                dictionaryFormObjects.Add(data.email, data);

            return dictionaryFormObjects;
        }

        public string DecompressBytes(byte[] compressedBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (StreamReader streamReader = new StreamReader(gzipStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
