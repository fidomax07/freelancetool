using System.ComponentModel.DataAnnotations;
using FreelanceTool.Helpers;
using FreelanceTool.Models;

namespace FreelanceTool.CustomValidators
{
	public class ResidencePermitRequiredAttribute : ValidationAttribute
    {
	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
			var applicant = (Applicant)validationContext.ObjectInstance;
		    if (applicant.NationalityId != Constants.NATIVE_NATIONALITY_ID &&
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
