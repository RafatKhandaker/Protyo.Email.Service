using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services.Contracts
{
    public interface IEmailService
    {
        IList<string> emailListing { get; set; }

    }
}
