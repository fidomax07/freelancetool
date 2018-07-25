﻿using System.ComponentModel.DataAnnotations;
using FreelanceTool.Helpers.Enums;
using FreelanceTool.Models;
using FreelanceTool.ViewModels;

namespace FreelanceTool.Helpers.CustomValidators
{
	public class CompanyNameConditionalRequiredAttribute : ValidationAttribute
	{
		private readonly Occupation _selfEmployedOccupation;

		public CompanyNameConditionalRequiredAttribute()
		{
			_selfEmployedOccupation = Occupation.SelfEmployed;
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
			return $"Company-name field is required if occupation is: {_selfEmployedOccupation}";
		}
	}
}