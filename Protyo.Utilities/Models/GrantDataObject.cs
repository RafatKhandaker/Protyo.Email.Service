using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Protyo.Utilities.Models
{
    public class GrantDataObject
    {
        [BsonId]
        public int? GrantId { get; set; }

        public string AgencyCode { get; set; }
        public string OppNumber { get; set; }
        public string Title { get; set; }
        public string Agency { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public string OppStatus { get; set; }
        public string DocType { get; set; }
        public List<string> CfdaList { get; set; }

        public GrantDetails? Details { get; set; }
    }
}
