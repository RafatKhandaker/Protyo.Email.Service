using Protyo.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.EmailSubscriptionService.Services
{
    public class ItemsMapper
    {
        public List<FormData> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<FormData>();
            foreach (var value in values)
            {
                if (value.Count > 0)
                {
                    FormData item = new()
                    {
                        timestamp = value[0].ToString(),
                        companyName = value[1].ToString(),
                        companyWebsite = value[2].ToString(),
                        yourName = value[3].ToString(),
                        email = value[4].ToString(),
                        address = value[5].ToString(),
                        phoneNumber = value[6].ToString(),
                        organizationProductService = value[7].ToString(),
                        organizationMissionGoals = value[8].ToString(),
                        organizationGrants = value[9].ToString(),
                        grantPurpose = value[10].ToString(),

                    };
                    items.Add(item);
                }
            }
            return items;
        }
        public IList<IList<object>> MapToRangeData(FormData item)
        {
            var objectList = new List<object>() { item.timestamp, item.companyName, item.companyWebsite, item.yourName, item };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
