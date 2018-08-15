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
		private readonly ApplicationDbContext _dataContext;
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _configuration;
		private readonly AppLocalizer _appLocalizer;
		private readonly IEmailSender _emailService;


		public TestController(
			ApplicationDbContext dataContext,
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
		public async Task<object> Index()
		{
			//foreach (var nationality in _dataContext.Nationalities)
			//{
			//	var natGerman = await _dataContext
			//		.NationalityDes
			//		.AsNoTracking()
			//		.SingleOrDefaultAsync(nd => nd.Alpha2 == nationality.Alpha2);
			//	if (natGerman == null) continue;

			//	nationality.NameGerman = natGerman.NameGerman;
			//}

			//await _dataContext.SaveChangesAsync();

			return "success";
		}
	}
}