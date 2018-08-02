using System.Linq;
using FreelanceTool.Data;
using FreelanceTool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
	{
		private readonly IConfiguration _config;
		private readonly ApplicationDataContext _dataContext;
		private readonly IEmailSender _emailService;


		public TestController(
			IConfiguration config,
			ApplicationDataContext dataContext, 
			IEmailSender emailService)
		{
			_config = config;
			_dataContext = dataContext;
			_emailService = emailService;
		}

		public string Index()
		{
			var applicant = _dataContext.Applicants
				.Include(a => a.ApplicantFiles)
				.SingleOrDefault(a => a.Id == 1);

			if (applicant != null)
				_emailService.SendNewApplicationEmailAsync(applicant);

			return _config.GetSection("targetEmailAddress").Value;
		}
	}
}