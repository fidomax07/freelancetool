using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceTool.Controllers
{
    public class TestController : Controller
    {
	    private string _applicantDateOfBirth;
	    public string ApplicantDateOfBirth
	    {
		    get => _applicantDateOfBirth;
		    set
		    {
			    DateTime.TryParseExact(
				    value,
				    "dd.MM.yyyy",
				    CultureInfo.InvariantCulture,
				    DateTimeStyles.None,
				    out var dateParsed);

			    _applicantDateOfBirth = dateParsed.ToString(CultureInfo.InvariantCulture);
			    ApplicantRealDateOfBirth = dateParsed;
		    }
	    }
		public DateTime ApplicantRealDateOfBirth { get; set; }

		public object Index()
		{
			//var parsed = DateTime.TryParseExact("25.07.1990", 
			// "dd.MM.yyyy", 
			// CultureInfo.InvariantCulture, 
			// DateTimeStyles.None,
			// out var dt);

			ApplicantDateOfBirth = "26.07.1990";
			return Json(ApplicantDateOfBirth);
        }
    }
}