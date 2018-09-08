using System.ComponentModel.DataAnnotations;
using FreelanceTool.Models;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.CustomValidators
{
	public class CompanyNameRequiredAttribute : ValidationAttribute
	{
		private readonly Occupation _selfEmployedOccupation;

		public CompanyNameRequiredAttribute()
		{
			_selfEmployedOccupation = Occupation.SE;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var applicant = (Applicant)validationContext.ObjectInstance;
			var currentOccupation = applicant.Occupation;
			if (currentOccupation == _selfEmployedOccupation &&
				string.IsNullOrWhiteSpace(value.ToString()))
			{
				return new ValidationResult(GetErrorMessage());
			}

			return ValidationResult.Success;
		}

		private string GetErrorMessage()
		{
			return "Company name is required when occupation is self-employed.";
		}
	}
}