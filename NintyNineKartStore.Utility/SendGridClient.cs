using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Utility
{
    public class SendGridClient:IEmailSender
    {
        private readonly IEmailSender _emailSender;
        public SendGridClient(IEmailSender _emailSender)
        {
            this._emailSender = _emailSender;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
