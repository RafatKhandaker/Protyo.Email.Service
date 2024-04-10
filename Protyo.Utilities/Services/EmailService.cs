using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Protyo.Utilities.Services
{
    public class EmailService : IEmailService
    {
        private string _smtpServer;

        private int _smtpPort;

        private string _smtpUsername;

        private string _smtpPassword;

        public IList<string> emailListing { get; set; }

        private IConfigurationSetting _configurationSetting;

        public EmailService(IConfigurationSetting configurationSetting) {
            _configurationSetting = configurationSetting;
            _smtpServer = _configurationSetting.appSettings["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(_configurationSetting.appSettings["EmailSettings:SmtpPort"]);
            _smtpUsername = _configurationSetting.appSettings["EmailSettings:Username"];
            _smtpPassword = _configurationSetting.appSettings["EmailSettings:Password"]; 
        }


        public async void send(string emailSubject, string emailBody) {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.EnableSsl = true; // Enable SSL/TLS
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

                foreach (var email in emailListing)
                {
                    var message = new MailMessage
                    {
                        From = new MailAddress(_smtpUsername),
                        Subject = emailSubject,
                        Body = emailBody,
                        IsBodyHtml = true
                    };
                    message.To.Add(email);

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
