using System.ComponentModel.DataAnnotations;
using FreelanceTool.Models;
using static FreelanceTool.Helpers.Constants;

namespace FreelanceTool.Helpers.CustomValidators
{
	public class ResidencePermitRequiredAttribute : ValidationAttribute
    {
	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
			var applicant = (Applicant)validationContext.ObjectInstance;
		    if (applicant.NationalityId != NATIVE_NATIONALITY_ID &&
		        string.IsNullOrWhiteSpace(value.ToString()))
		    {
				return new ValidationResult(GetErrorMessage());
		    }

			return ValidationResult.Success;
		}

	    private string GetErrorMessage()
	    {
		    return "Residence permit is required when selected country is not Switzerland.";
	    }
    }
}
