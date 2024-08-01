using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Models
{
    public class GrantMatch
    {
        public int? GrantId { get; set; }
        public int? Grade { get; set; }
        public string Details { get; set; }
        public GrantDataObject Grant { get; set; }
    }
}
