using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Shop.ServicesFacade.Concrete.Common;

namespace Shop.Services.Concrete.Common
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}