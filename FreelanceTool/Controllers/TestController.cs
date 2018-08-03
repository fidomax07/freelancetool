using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using Microsoft.AspNetCore.Localization;
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
		private readonly AppLocalizer _localizer;
		private readonly PathHandler _pathService;


		public TestController(
			IConfiguration config,
			ApplicationDataContext dataContext,
			IEmailSender emailService,
			AppLocalizer localizer,
			PathHandler pathService)
		{
			_config = config;
			_dataContext = dataContext;
			_emailService = emailService;
			_localizer = localizer;
			_pathService = pathService;
		}

		public async Task<string> Index()
		{
			var currentCulture = HttpContext.Features
				.Get<IRequestCultureFeature>()
				.RequestCulture
				.UICulture;

			var applicant = await _dataContext.Applicants
				.Include(a => a.MainLanguage)
				.Include(a => a.Nationality)
				.Include(a => a.SpokenLanguages)
				.Include(a => a.JsTrainingCertificates)
				.Include(a => a.ApplicantFiles)
				.SingleOrDefaultAsync(a => a.Id == 1);


			var csvModel = new CsvModel(applicant);
			//.MapComplexProperty(applicant,
			//	nameof(applicant.MainLanguage),
			//	nameof(applicant.MainLanguage.NameEnglish))
			//.MapComplexProperty(applicant,
			//	nameof(applicant.Nationality),
			//	nameof(applicant.Nationality.NameEnglish));

			var csvContent = new StringBuilder();
			var csvProperties = csvModel.GetType().GetProperties();
			// Write headers first
			foreach (var propInfo in csvProperties)
			{
				if (propInfo.GetValue(csvModel) == null)
					continue;

				var localizedPropName = _localizer
					.LocalizeClassMember<CsvModel>(propInfo.Name);
				csvContent.Append($"{localizedPropName}|");
			}
			// Remove last pipe
			csvContent.Remove(csvContent.Length - 1, 1);
			csvContent.AppendLine();

			// Write values next
			foreach (var propInfo in csvProperties)
			{
				var propType = propInfo.PropertyType;
				var propValue = propInfo.GetValue(csvModel);

				if (propValue == null)
					continue;

				// Handle language value retrieving
				if (propType == typeof(Language))
				{
					var propValueLocalized = csvModel
						.MainLanguage
						.GetLocalizedName(currentCulture);
					csvContent.Append($"\"{propValueLocalized}\"|");

					continue;
				}

				// Handle nationality value retrieving
				if (propType == typeof(Nationality))
				{
					var propValueLocalized = csvModel
						.Nationality
						.GetLocalizedName(currentCulture);
					csvContent.Append($"\"{propValueLocalized}\"|");

					continue;
				}

				// Handle strings value retrieving
				if (propType == typeof(string))
				{
					csvContent.Append($"\"{propValue}\"|");

					continue;
				}

				// Handle enums value retrieving
				if (propType.IsEnum)
				{
					var propValueCasted = propValue as Enum;
					var propValueLocalized = _localizer
						.LocalizeEnum(propValueCasted);
					csvContent.Append($"{propValueLocalized}|");

					continue;
				}

				csvContent.Append($"{propValue}|");
			}


			// Write to Csv file
			var path = Path.Combine(
				_pathService.GetCsvPath(),
				$"{applicant.Id.ToString()}.csv");
			using (var outputFile = new StreamWriter(path, false, Encoding.UTF8))
			{
				outputFile.Write(csvContent.ToString());
			}

			return csvContent.ToString();
		}
	}
}