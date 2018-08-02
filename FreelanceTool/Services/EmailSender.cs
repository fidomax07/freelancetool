using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FreelanceTool.Helpers;
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
			var client = new SmtpClient("smtp.office365.com")
			{
				Port = 587,
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("fidomax07@hotmail.com", "")
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
				From = new MailAddress("fidomax07@hotmail.com"),
				To = { _config.GetSection("targetEmailAddress").Value },
				Subject = "Neue Freelancer-Anmeldung",
				IsBodyHtml = true,
				Body = body
			};

			// Attach CSV file
			var filePath = Path.Combine(
				_env.ContentRootPath,
				Constants.UPLOAD_PATH,
				Constants.CSV_PATH,
				applicant.Csv.UniqueName);
			using(var stream = new FileStream(filePath, FileMode.Open))
			{
				mailMessage.Attachments.Add(
					new Attachment(stream, "CSVData", "text/csv"));
				await client.SendMailAsync(mailMessage);
			}
		}
	}
}
