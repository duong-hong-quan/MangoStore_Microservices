using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {

        private DbContextOptions<AppDBContext> _dbOptions;

        public EmailService(DbContextOptions<AppDBContext> dbOptions)
        {
            this._dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cart)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/> Cart Email Requested");
            message.AppendLine("<br/> Total" + cart.CartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<ul>");
            foreach (var item in cart.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cart.CartHeader.Email);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new EmailLogger
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message,

                };
                await using var _db = new AppDBContext(_dbOptions);
                _db.EmailLoggers.Add(emailLogger);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
