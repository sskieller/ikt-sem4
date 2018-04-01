using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace FWPS.Controllers
{
    public static class MailSender
    {

        public static void SendSnapBoxMail(FwpsDbContext context, SnapBoxItem item)
        {
            MailMessage mail = new MailMessage("simonvu@post.au.dk", item.ReceiverEmail);
            mail.Body = string.Format("You have received mail\nAlso your current power level is: {0}", item.PowerLevel);
            mail.Subject = "New mail in SnapBox";

            MailItem mailCopy = new MailItem()
            {
                To = item.ReceiverEmail,
                From = "simonvu@post.au.dk",
                Body = string.Format("You have received mail\nAlso your current power level is: {0}", item.PowerLevel),
                Subject = "New mail in SnapBox"
            };

            context.MailItems.Add(mailCopy);
            context.SaveChanges();

            //Send mail here
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("au553622@uni.au.dk", "4Doru0109");
            client.Host = "post.au.dk";

            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure to send email");
            }
            



        }


    }
}
