using System;
using System.Collections.Generic;

namespace Protyo.Utilities.Services.Contracts
{
    public interface IEmailService
    {
        public IList<string> emailListing { get; set; }
        public void send(string emailSubject, string emailBody);
        public void sendHtmlFromFilePath(string emailSubject, string htmlFilePath);
    }
}
