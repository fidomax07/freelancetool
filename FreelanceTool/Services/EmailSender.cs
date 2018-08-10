using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FreelanceTool.Services
{
	// This class is used by the application to send email for account confirmation and password reset.
	// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
	public class EmailSender : IEmailSender
	{
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _config;

		public EmailSender(IHostingEnvironment env, IConfiguration configuration)
		{
			_env = env;
			_config = configuration;
		}

		public Task SendEmailAsync(string email, string subject, string message)
		{
			return Task.CompletedTask;
		}

		public async Task SendNewApplicationEmailAsync(Applicant applicant)
		{
			var client = new SmtpClient(_config.GetSection("emailHost").Value)
			{
				Port = 587,
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(
					_config.GetSection("senderEmailAddress").Value,
					_config.GetSection("senderEmailPassword").Value)
			};
			var body = $@"
				<h2>Hallo!</h2>
				<p>Es ist eine neue Freelancer-Anmeldung eingegangen.</p>
				<p>Anbei des Import-File.</p>
				<p>Datenkbank -ID: <b>{applicant.Id}</b></p>
				<p>Datum: {DateTime.Now.ToStringLocale()}</p>
			";
			var mailMessage = new MailMessage
			{
				From = new MailAddress(_config.GetSection("senderEmailAddress").Value),
				To = { _config.GetSection("targetEmailAddress").Value },
				Subject = "Neue Freelancer-Anmeldung",
				IsBodyHtml = true,
				Body = body
			};

			// Attach CSV file
			var csvName = applicant.Csv.UniqueName;
			var filePath = Path.Combine(
				PathHandler.GetCsvPath(_env), csvName);
			using(var stream = new FileStream(filePath, FileMode.Open))
			{
				var att = new Attachment(stream, csvName, "text/csv");
				mailMessage.Attachments.Add(att);

				await client.SendMailAsync(mailMessage);
			}
		}
	}
}
