using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Photograph.BLL.Services.MailService
{
	public class MailService : IEmailService
	{
		public void SendEmail(string subject, string body, string receiverEmailAddress)
		{
			var smtpClient = GetSmtpClient();
			var mail = CreateMailMessage(subject, body, receiverEmailAddress);

			//smtpClient.Send(mail);

			mail.Dispose();
			smtpClient.Dispose();
		}

		public string CreateEmailBody(string username, string template)
		{ 
			var body = string.Empty;
			var runningPath = AppDomain.CurrentDomain.BaseDirectory;

			using (var reader = new StreamReader($"{runningPath}\\Resources\\{template}.html"))
			{
				body = reader.ReadToEnd();
			}

			body = body.Replace("{Username}", username);

			return body;
		}

		private MailMessage CreateMailMessage(string subject, string body, string receiverEmailAddress)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress(ConfigurationManager.AppSettings["Username"]),
				Subject = subject,
				Body = body,
				IsBodyHtml = true 
			};
			mailMessage.To.Add(new MailAddress(receiverEmailAddress));

			return mailMessage;
		}

		private SmtpClient GetSmtpClient()
		{
			var credentials = new NetworkCredential
			{
				UserName = ConfigurationManager.AppSettings["Username"],
				Password = ConfigurationManager.AppSettings["Password"]
			};

			var client = new SmtpClient
			{
				Host = ConfigurationManager.AppSettings["Host"],
				Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
				UseDefaultCredentials = false,
				Credentials = credentials,
				EnableSsl = true,
			};

			return client;
		}
	}
}