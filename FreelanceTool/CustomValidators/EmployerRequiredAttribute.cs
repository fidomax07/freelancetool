using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FreelanceTool.Models;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.CustomValidators
{
	public class EmployerRequiredAttribute : ValidationAttribute
    {
	    private readonly IEnumerable<Occupation> _occupationEmployerValues;

	    public EmployerRequiredAttribute()
	    {
			_occupationEmployerValues = new List<Occupation>
				{ Occupation.PT, Occupation.FT };
		}

	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
		    var applicant = (Applicant) validationContext.ObjectInstance;
			if (_occupationEmployerValues.Contains(applicant.Occupation) &&
			    string.IsNullOrWhiteSpace(value.ToString()))
		    {
			    return new ValidationResult(GetErrorMessage());
			}

			return ValidationResult.Success;
		}

	    //public void AddValidation(ClientModelValidationContext context)
	    //{
		   // if (context == null)
			  //  throw new ArgumentNullException(nameof(context));

		   // MergeAttribute(context.Attributes, "data-val", "true");
		   // MergeAttribute(context.Attributes, "data-val-employer-conditional-required", GetErrorMessage());

		   // MergeAttribute(context.Attributes, 
			  //  "data-val-occupation-employer-values", 
			  //  JsonConvert.SerializeObject(_occupationEmployerValues));
	    //}

	    //private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
	    //{
		   // if (attributes.ContainsKey(key))
		   // {
			  //  return false;
		   // }

		   // attributes.Add(key, value);
		   // return true;
	    //}

		private string GetErrorMessage()
	    {
		    return "Employer is required when occupation is part or full-time.";
	    }
    }
}
