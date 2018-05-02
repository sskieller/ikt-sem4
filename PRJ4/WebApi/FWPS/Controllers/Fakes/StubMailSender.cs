using System;
using FWPS.Data;
using FWPS.Models;

namespace FWPS.Controllers
{
    public class SendSnapBoxMailExecutedException : Exception
    {
    }

    public class StubMailSender : IMailSender
    {
        private readonly FwpsDbContext _context;
        public StubMailSender(FwpsDbContext context)
        {
            _context = context;
        }
        public void SendSnapBoxMail(SnapBoxItem item) => throw new SendSnapBoxMailExecutedException();
    }
}