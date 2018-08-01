using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool
{
    public class TestingCsv
    {
	    private readonly ApplicationDataContext _context;
	    private readonly AppLocalizer _localizer;
	    private readonly PathHandler _pathService;

		public TestingCsv(ApplicationDataContext context,
			AppLocalizer localizer,
			PathHandler pathService)
	    {
		    _context = context;
		    _localizer = localizer;
		    _pathService = pathService;
		}

	    public async Task TestMethodCsv()
	    {
		    var csvValues = new[]
		    {
			    nameof(Applicant.LastName),
			    nameof(Applicant.FirstName),
			    nameof(Applicant.DateOfBirth),
			    nameof(Applicant.Country),
		    };

		    var applicant = await _context.Applicants
			    .Include(a => a.MainLanguage)
			    .Include(a => a.Nationality)
			    .Include(a => a.SpokenLanguages)
			    .Include(a => a.JsTrainingCertificates)
			    .Include(a => a.ApplicantFiles)
			    .SingleOrDefaultAsync(a => a.Id == 1);

		    //var mainLanguage = applicant.GetType()
		    //	.GetProperty("MainLanguage")
		    //	.GetValue(applicant) as Language;

		    var csvBuilder = new StringBuilder();

		    // Write headers first
		    foreach (var propInfo in applicant.GetType().GetProperties())
		    {
			    if (csvValues.Contains(propInfo.Name))
			    {
				    var localizedPropName = _localizer
					    .LocalizeClassMember<Applicant>(propInfo.Name);
				    csvBuilder.Append($"\"{localizedPropName}\"|");
			    }
		    }
		    // Remove last pipe
		    csvBuilder.Remove(csvBuilder.Length - 1, 1);
		    csvBuilder.AppendLine();

		    // Write values next
		    csvBuilder.Append($"\"{applicant.LastName}\"|");
		    csvBuilder.Append($"\"{applicant.FirstName}\"|");
		    csvBuilder.Append($"{applicant.DateOfBirth:dd.MM.yyyy}|");
		    csvBuilder.Append($"\"{_localizer.LocalizeEnum(applicant.Country)}\"");

		    var path = Path.Combine(
			    _pathService.GetCsvPath(),
			    $"{applicant.Id.ToString()}.csv");
		    using (var outputFile = new StreamWriter(path, false, Encoding.UTF8))
		    {
			    outputFile.Write(csvBuilder.ToString());
		    }
		}
    }
}
