using System.Net.Mail;

namespace Photograph.BLL.Services.MailService
{
	public interface IEmailService
	{
		void SendEmail(string subject, string body, string receiverEmailAddress);

		string CreateEmailBody(string userName, string template);
	}
}