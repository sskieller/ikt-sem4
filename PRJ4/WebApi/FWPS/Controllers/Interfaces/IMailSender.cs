using FWPS.Models;

namespace FWPS.Controllers
{
    interface IMailSender
    {
        void SendSnapBoxMail(SnapBoxItem item);
    }
}