using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;


namespace WebApplication14.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunctionController : Controller
    {

        public FunctionController(IConfiguration configuration)
        {

        }


        [HttpPost]
        [AllowAnonymous]


        public async Task<IActionResult> Send_Mail([FromBody] FormData data)
        {


            string rich_text;
            try
            {
          string body =  @"<!DOCTYPE html>
<html lang='he'>
<head>
    <meta charset='UTF-8'>
<style>
p {
    margin: 0;
}</style>
</head>
<body dir='rtl' style='font-family: Arial; text-align: right;'> <p>
שם הלקוח:" + data.name + "</p><p>" + "טלפון הלקוח:" + data.phone + "</p><p>" + ((data.message != "") ? ("מעונין ב-" + data.message) : "") + "</p><p>" + ((data.email_contact != "") ? ("מייל הלקוח:" + data.email_contact) : "") + "</p></body></html>";
    
                MailMessage message = new MailMessage(new MailAddress(Environment.GetEnvironmentVariable("send_mail")), new MailAddress("y0556722091@gmail.com"));


                message.Subject = "פנית לקוח";
                message.Body = body;

                message.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {

                    client.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("send_mail"), Environment.GetEnvironmentVariable("pass_mail"));
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    await client.SendMailAsync(message);
                }
            
                    rich_text = "<h3>פנייתך התקבלה בהצלחה</h3>\r\n<p>נציגנו יתפנו בהקדם ויטפלו בבקשתכם</p>";
                    return Ok(new { success = true, message = rich_text });
             

            }
            catch (Exception ex)
            {
        
                rich_text = "<h3 style=\"color: red;\">פנייתך נכשלה</h3>\r\n<p>אנא נסה שוב מאוחר יותר</p>";
                    return StatusCode(400, new { success = false, message = rich_text });
            }

        }
        public class FormData
        {
            public string name { get; set; }
            public string? email_contact { get; set; }
            public string phone { get; set; }
            public string? message { get; set; }
        }
    }
}
