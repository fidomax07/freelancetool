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
		        applicant.ResidencePermit == null)
		    {
				//return new ValidationResult(GetErrorMessage());
				return new ValidationResult(ErrorMessage);
		    }

			return ValidationResult.Success;
		}

	    /*private string GetErrorMessage()
	    {
		    return "Residence permit is required when selected country is not Switzerland.";
	    }*/
    }
}
