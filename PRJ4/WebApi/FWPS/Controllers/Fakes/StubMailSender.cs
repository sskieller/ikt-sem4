using System;
using FWPS.Data;
using FWPS.Models;

namespace FWPS.Controllers
{
    /////////////////////////////////////////////////
    /// Interface for mail sender, not used
    /////////////////////////////////////////////////
    public interface IMailSender
    {
        void SendSnapBoxMail(SnapBoxItem item);
    }
    public class SendSnapBoxMailExecutedException : Exception
    {
    }
    /////////////////////////////////////////////////
    /// Sub class for mail sender, not used
    /////////////////////////////////////////////////
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