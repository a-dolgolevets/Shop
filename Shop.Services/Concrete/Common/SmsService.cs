using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Shop.ServicesFacade.Concrete.Common;

namespace Shop.Services.Concrete.Common
{
    public class SmsService : ISmsService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}