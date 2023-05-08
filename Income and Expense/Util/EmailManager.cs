//using Income_and_Expense.ViewModels;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.IO;
//using System.Net.Mail;
//using System.Net;
//using System.Threading.Tasks;
//using Humanizer;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using static Humanizer.On;
//using System.Data;
//using System.Reflection.Metadata;
//using System.Runtime.InteropServices;
//using System.Xml.Linq;

//namespace Income_and_Expense.Util
//{
//    public class EmailManager
//    {
//        public static string PathName(string sectionName)
//        {
//            var configurationBuilder = new ConfigurationBuilder();
//            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
//            configurationBuilder.AddJsonFile(jsonPath);
//            var root = configurationBuilder.Build();
//            return root.GetSection("SMTP").GetSection(sectionName).Value;
//        }

//        public static async Task<bool> ReadyEmail(EmailReadyVM emailReadyVM)
//        {
//            var detailText = @"<tr style='background:white;'>
//                        <td colspan=""3"" style=""text-align:left;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:15px;font-size:14px;"">
//                            Thank you for considering <b> Odyssey Design </b> for your <b> #ServiceName# Project. </b> We appreciate the opportunity to work with you on this project.
//                        </td>
//                    </tr>
//                    <tr style='background:white;'>
//                        <td colspan=""3"" style=""text-align:left;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:25px;font-size:14px;"">
//                            Below are the details of your selected service and package:
//                        </td>
//                    </tr>
//                    <tr style=""background-image:url('https://imgur.com/2IoqOqm.png');background-size:cover;"">
//                        <th style=""color:white;font-family:arial;padding:17px 0px;"">Service Name</th>
//                        <th style=""color:white;font-family:arial;padding:17px 0px;"">Selected Package</th>
//                        <th style=""color:white;font-family:arial;padding:17px 0px;"">Estimate Pricing</th>
//                    </tr>
//                    <tr style='background:white;'>
//                        <th style=""color:white;font-family:arial;padding:5px 0px;""></th>
//                        <th style=""color:white;font-family:arial;padding:5px 0px;""></th>
//                        <th style=""color:white;
//font-family:arial;padding:5px 0px;""></th>
//                    </tr>
//                    <tr style=""background-image:url('https://imgur.com/12L2aM0.png');background-size:cover;"">
//                    <th style=""color:black;font-family:arial;padding:22px 0px;"">#ServiceName#</th>
//                    <th style=""color:black;font-family:arial;padding:22px 0px;"">#SelectedPackage#</th>
//                    <th style=""color:black;font-family:arial;padding:22px 0px;"">#EstimatePricing#</th>
//                    </tr>
//                    <tr style='background:white;'>
//                        <td colspan=""3"" style=""text-align:center;padding-left:25px;padding-top:25px;font-family:arial;padding-bottom:15px;font-size:
//14px;"">
//                            Let's schedule a meeting that suits your schedule, Contact us at <a style=""color:blue;text-decoration:underline;""> 210-446-9069 </a> or <a style=""color:blue;text-decoration:underline;"">odysseydesignco@gmail.com </a>
//                        </td>
//                    </tr>
//                    <!-- <tr><td colspan=""3"" style=""text-align:center;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:15px;font-size:14px;""> <button style=""background:hsl(204deg 100% 50%);padding:15px 33px;border-radius:31px;border:0;color:white;font-size:16px;""> Shedule a Meeting </button>
//                    </td></tr> -->
//                    <tr style='background:white;'>
//                        <td colspan=""3"" style=""text-align:center;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:24px;font-size:12px;"">
//                            No scheduling necessary, Save time on scheduling. We'll be in touch within 12-24 hours to talk about your project.
//                        </td>
//                    </tr>";

//            // string webRootPath = _webHostEnvironment.WebRootPath;
//            var pathInput = Path.Combine(emailReadyVM.Path + "\\EmailTemplate", "EmailLayout.html");
//            StreamReader str = new StreamReader(pathInput);
//            string body = str.ReadToEnd();
//            str.Close();
//            string subject = "";
//            string toEmails = "";

//            body = body.Replace("#UserTitle#", emailReadyVM.UserTitle);

//            if (emailReadyVM.EmailTypeId == 1)
//            {
//                toEmails = emailReadyVM.ToEmail + "," + PathName("ToEmails");
//                body = body.Replace("[DetailText]", detailText);
//                subject = @"Odyssey Design: Your " + emailReadyVM.Subject + " Quote is Here!";
//                body = body.Replace("#ServiceName#", emailReadyVM.ServiceName);
//                body = body.Replace("#SelectedPackage#", emailReadyVM.SelectedPackage);
//                body = body.Replace("#EstimatePricing#", emailReadyVM.EstimatePricing);
//            }
//            if (emailReadyVM.EmailTypeId == 2)
//            {
//                string emailBody = @"<tr style='background:white;'>
//                        <td colspan=""3"" style=""text-align:left;padding-left:25px;padding-top:10px;font-family:arial;padding-bottom:15px;font-size:14px;"">
//                          #EmailBody#
//                        </td>
//                    </tr>";
//                emailBody = emailBody.Replace("#EmailBody#", emailReadyVM.Body);
//                toEmails = emailReadyVM.ToEmail;
//                body = body.Replace("[DetailText]", emailBody);
//                subject = emailReadyVM.Subject;
//            }

//            bool obj = false;

//            foreach (var email in toEmails.Split(","))
//            {
//                var emailDetail = new EmailDetailVM()
//                {
//                    ToEmail = email,
//                    Subject = subject,
//                    Body = body,
//                };

//                obj = await SendEmail(emailDetail);
//            }
//            return obj;
//        }

//        public static async Task<bool> SendEmail(EmailDetailVM emailDetailVM)
//        {
//            try
//            {


//                MailAddress from = new MailAddress(PathName("From"), PathName("FromName"));
//                MailAddress to = new MailAddress(emailDetailVM.ToEmail);
//                MailMessage message = new MailMessage(from, to);
//                var Url = PathName("Url"); //root.GetSection("SMTP").GetSection("Url").Value; 
//                message.Body = "This is test Body";
//                message.Subject = "Request for Leave";
//                message.IsBodyHtml = true;
//                string server = PathName("Server");
//                SmtpClient client = new SmtpClient(server);
//                client.Port = Convert.ToInt16(PathName("Port"));
//                client.EnableSsl = Convert.ToBoolean(PathName("EnableSsl"));
//                client.UseDefaultCredentials = Convert.ToBoolean(PathName("UseDefaultCredentials"));
//                client.Credentials = new NetworkCredential(PathName("UserName"), PathName("Password"));
//                client.Timeout = Convert.ToInt16(PathName("Timeout"));
//                await client.SendMailAsync(message);



//            }
//            catch (Exception ex)
//            {

//            }
//            return true;
//        }
//    }
//}



//////////
/////

////< !DOCTYPE html >
////< html lang = "en" xmlns = "http://www.w3.org/1999/xhtml" xmlns: o = "urn:schemas-microsoft-com:office:office" >
////< head >
////  < meta charset = "UTF-8" >
////  < meta name = "viewport" content = "width=device-width,initial-scale=1" >
////  < meta name = "x-apple-disable-message-reformatting" >
////  < title ></ title >
////  < !--[if mso]>
////  < noscript >
////    < xml >
////      < o:OfficeDocumentSettings >
////        < o:PixelsPerInch > 96 </ o:PixelsPerInch >
////      </ o:OfficeDocumentSettings >
////    </ xml >
////  </ noscript >
////  < ![endif]-- >
////  < style >
////    table, td, div, h1, p
////{ font - family: Arial, sans - serif; }
////  </ style >
////</ head >
////< body style = "margin:0;padding:0;" >
////  < table role = "presentation" style = "width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;" >
////    < tr >
////      < td align = "center" style = "padding:0;" >
////        < table role = "presentation" style = "width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;" >
////          < tr >
////            < td align = "center" style = "padding:40px 0 30px 0;background:#70bbd9;" >
////              < img src = "https://assets.codepen.io/210284/h1.png" alt = "" width = "300" style = "height:auto;display:block;" />
////            </ td >
////          </ tr >
////          < tr >
////            < td style = "padding:36px 30px 42px 30px;" >
////              < table role = "presentation" style = "width:100%;border-collapse:collapse;border:0;border-spacing:0;" >
////                < tr >
////                  < td style = "padding:0 0 36px 0;color:#153643;" >
////                    < h1 style = "font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;" > Creating Email Magic</h1>
////                    <p style = "margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" > Lorem ipsum dolor sit amet, consectetur adipiscing elit.In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan et dictum, nisi libero ultricies ipsum, posuere neque at erat.</p>
////                    <p style = "margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" >< a href= "http://www.example.com" style= "color:#ee4c50;text-decoration:underline;" > In tempus felis blandit</a></p>
////                  </td>
////                </tr>
////                <tr>
////                  <td style = "padding:0;" >
////                    < table role= "presentation" style= "width:100%;border-collapse:collapse;border:0;border-spacing:0;" >
////                      < tr >
////                        < td style= "width:260px;padding:0;vertical-align:top;color:#153643;" >
////                          < p style= "margin:0 0 25px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" >< img src= "https://assets.codepen.io/210284/left.gif" alt= "" width= "260" style= "height:auto;display:block;" /></ p >
////                          < p style= "margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" > Lorem ipsum dolor sit amet, consectetur adipiscing elit.In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan dictum, est nisi libero ultricies ipsum, in posuere mauris neque at erat.</p>
////                          <p style = "margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" >< a href= "http://www.example.com" style= "color:#ee4c50;text-decoration:underline;" > Blandit ipsum volutpat sed</a></p>
////                        </td>
////                        <td style = "width:20px;padding:0;font-size:0;line-height:0;" > &nbsp;</ td >
////                        < td style = "width:260px;padding:0;vertical-align:top;color:#153643;" >
////                          < p style = "margin:0 0 25px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" >< img src = "https://assets.codepen.io/210284/right.gif" alt = "" width = "260" style = "height:auto;display:block;" /></ p >
////                          < p style = "margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;" > Morbi porttitor, eget est accumsan dictum, nisi libero ultricies ipsum, in posuere mauris neque at erat. Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed.</p>
////                          <p style="margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;"><a href="http://www.example.com" style="color:#ee4c50;text-decoration:underline;">In tempus felis blandit</a></p>
////                        </td>
////                      </tr>
////                    </table>
////                  </td>
////                </tr>
////              </table>
////            </td>
////          </tr>
////          <tr>
////            <td style="padding:30px;background:#ee4c50;">
////              <table role="presentation" style="width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;">
////                <tr>
////                  <td style="padding:0;width:50%;" align="left">
////                    <p style="margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;">
////                      &reg; Someone, Somewhere 2021 < br />< a href = "http://www.example.com" style = "color:#ffffff;text-decoration:underline;" > Unsubscribe </ a >
////                    </ p >
////                  </ td >
////                  < td style = "padding:0;width:50%;" align = "right" >
////                    < table role = "presentation" style = "border-collapse:collapse;border:0;border-spacing:0;" >
////                      < tr >
////                        < td style = "padding:0 0 0 10px;width:38px;" >
////                          < a href = "http://www.twitter.com/" style = "color:#ffffff;" >< img src = "https://assets.codepen.io/210284/tw_1.png" alt = "Twitter" width = "38" style = "height:auto;display:block;border:0;" /></ a >
////                        </ td >
////                        < td style = "padding:0 0 0 10px;width:38px;" >
////                          < a href = "http://www.facebook.com/" style = "color:#ffffff;" >< img src = "https://assets.codepen.io/210284/fb_1.png" alt = "Facebook" width = "38" style = "height:auto;display:block;border:0;" /></ a >
////                        </ td >
////                      </ tr >
////                    </ table >
////                  </ td >
////                </ tr >
////              </ table >
////            </ td >
////          </ tr >
////        </ table >
////      </ td >
////    </ tr >
////  </ table >
////</ body >
////</ html >