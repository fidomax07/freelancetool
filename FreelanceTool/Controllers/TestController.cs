using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
	{
		private readonly ApplicationDataContext _dataContext;
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _configuration;
		private readonly AppLocalizer _appLocalizer;
		private readonly IEmailSender _emailService;


		public TestController(
			ApplicationDataContext dataContext,
			IHostingEnvironment env,
			IConfiguration configuration,
			AppLocalizer appLocalizer,
			IEmailSender emailService)
		{
			_dataContext = dataContext;
			_env = env;
			_configuration = configuration;
			_appLocalizer = appLocalizer;
			_emailService = emailService;
		}

		[AllowAnonymous]
		public object Index()
		{
			return _configuration.GetSection("emailHost");
		}
	}
}