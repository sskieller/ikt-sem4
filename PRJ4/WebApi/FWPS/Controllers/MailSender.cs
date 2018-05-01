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
    public class MailSender
    {
        private readonly FwpsDbContext _context;

        public MailSender(FwpsDbContext context)
        {
            _context = context;
        }

        public void SendSnapBoxMail(SnapBoxItem item)
        {
            MailMessage mail = new MailMessage("simonvu8210@gmail.com", item.ReceiverEmail);
            mail.Body = string.Format("You have received mail\nAlso your current power level is: {0}", item.PowerLevel);
            mail.Subject = "New mail in SnapBox";

            MailItem mailCopy = new MailItem()
            {
                To = item.ReceiverEmail,
                From = "simonvu8210@gmail.com",
                Body = string.Format("You have received mail\nAlso your current power level is: {0}", item.PowerLevel),
                Subject = "New mail in SnapBox"
            };

            _context.MailItems.Add(mailCopy);
            _context.SaveChanges();

            //Send mail here
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("simonvu8210@gmail.com", "Kalle210");
            client.Host = "smtp.gmail.com";

            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                mailCopy.Subject = "FAILED TO SEND MAIL";
                _context.SaveChanges();
                Console.WriteLine("Failure to send email -- ", e.Message);
            }
            



        }


    }
}
