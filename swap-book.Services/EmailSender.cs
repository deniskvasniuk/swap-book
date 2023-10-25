﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace swap_book.Services
{

    public interface IEmailSender
    {
        void SendMail(string firstname, string lastname, string email, string msg);
    }

    public class EmailSender : IEmailSender
    {
        readonly ILogger<EmailSender> _logger;
        readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }

        public void SendMail(string firstname, string lastname, string email, string msg)
        {
            var settings = _configuration.GetSection("SmtpSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SwapBook", "swap.book.tech@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Thanks for your feedback";

            var currentDirectory = Directory.GetCurrentDirectory();

            var relativePath = Path.Combine("wwwroot", "mail", "feedback.html");

            var fullPath = Path.Combine(currentDirectory, relativePath);

            var htmlTemplate = File.ReadAllText(fullPath);

   
            htmlTemplate = htmlTemplate.Replace("{firstname}", firstname)
                .Replace("{lastname}", lastname)
                .Replace("{msg}", msg);

            message.Body = new TextPart("html")
            {
                Text = htmlTemplate
            };


            try
            {
                using (var client = new SmtpClient())
                {

                    var smtpServer = settings["SmtpServer"];
                    var smtpPort = int.Parse(settings["SmtpPort"]);
                    var smtpUsername = settings["SmtpUsername"];
                    var smtpPassword = settings["SmtpPassword"];

                    client.Connect(smtpServer, smtpPort, true);
                    client.Authenticate(smtpUsername, smtpPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
        }
    }
}


