﻿using Income_and_Expense.Data.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;

namespace Income_and_Expense.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();

            //From
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));

            //To
            email.To.Add(MailboxAddress.Parse(request.To));

            //Subject
            email.Subject = request.Subject;
            email.Subject = "Splitwise balance email";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = /*"<h1>HTML content of the email</h1>";*/
               "\"<tr style='background:white;'>\r\n         " +
               " <td colspan=\"\"3\"\" style=\"\"text-align:left;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:15px;font-size:14px;\"\">\r\n            " +
               "Hey Mr/Miss ! FYl: Mohsin just recorded a payment to you on Solowise. We appreciate the opportunity to work with you.\r\n     " +
               "  </td>\r\n " +
               " </tr>" +

               "<tr style='background:white;'>\r\n        " +
               "<td colspan=\"3\" style=\"text-align:left;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:25px;font-size:14px;\">\r\n     " +
               "Below are the details of your services:\r\n                        </td>\r\n                    </tr>" +

               "<tr style=\"background-image:url('https://imgur.com/2IoqOqm.png');background-size:cover;\">\r\n" +
               "                        <th style=\"color:white;font-family:arial;padding:17px 0px;\">Group Name</th>\r\n  " +
               "                      <th style=\"color:white;font-family:arial;padding:17px 0px;\">Balance</th>\r\n  " +
               "                      <th style=\"color:white;font-family:arial;padding:17px 0px;\">split</th>\r\n " +
               "                   </tr>" +
               "<tr style='background:white;'>\r\n                " +
               "  <th style=\"color:white;font-family:arial;padding:5px 0px;\"></th>\r\n  " +
               "   <th style=\"color:white;font-family:arial;padding:5px 0px;\"></th>\r\n " +
               "   <th style=\"color:white;\r\nfont-family:arial;padding:5px 0px;\"></th>\r\n      " +
               "              </tr>" +
               "<tr style=\"background-image:url('https://imgur.com/12L2aM0.png');background-size:cover;\">\r\n   " +
               "                 <th style=\"color:black;font-family:arial;padding:22px 0px;\">#khumaryan#</th>\r\n" +
               "                    <th style=\"color:black;font-family:arial;padding:22px 0px;\">#2000#</th>\r\n  " +
               "                  <th style=\"color:black;font-family:arial;padding:22px 0px;\">#food#</th>\r\n   " +
               "                 </tr>" +
               "<tr style='background:white;'>\r\n       " +
               "<td colspan=\"3\" style=\"text-align:center;padding-left:25px;padding-top:25px;font-family:arial;padding-bottom:15px;font-size:\r\n14px;\">\r\n      " +
               "Let's schedule a meeting that suits your schedule, Contact us at <a style=\"color:blue;text-decoration:underline;\"> 0938-210377 </a> or <a style=\"color:blue;text-decoration:underline;\">mmohsinkhan762@gmail.com </a>\r\n   " +
               "</td>\r\n        " +
               " </tr>" +
               "<!-- <tr><td colspan=\"3\" style=\"text-align:center;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:15px;font-size:14px;\"> <button style=\"background:hsl(204deg 100% 50%);padding:15px 33px;border-radius:31px;border:0;color:white;font-size:16px;\"> Shedule a Meeting </button>\r\n  " +
               " </td></tr> -->\r\n       " +
               "<tr style='background:white;'>\r\n           " +
               "  <td colspan=\"3\" style=\"text-align:center;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:24px;font-size:12px;\">\r\n " +
               " If you’re confused about this, just hit reply to send an email to you and figure out what’s going on.\r\n    " +
               "   </td>\r\n   " +
               "   </tr>";

            bodyBuilder.TextBody = "Plain text content of the email";
            email.Body = bodyBuilder.ToMessageBody();



            //Body
            //email.Body = new TextPart(TextFormat.Html)
            //{
            //    Text = request.Body
            //};

            //configuration SMTP
            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:port").Value),
                SecureSocketOptions.StartTls
                );

            //Adding Authenticate of the email
            smtp.Authenticate(_configuration.GetSection("Email:UserName").Value, _configuration.GetSection("Email:Password").Value);

            //Send Email
            smtp.Send(email);

            //Disconnnect of Server
            smtp.Disconnect(true);

        }

    }
}
