﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FreelanceTool.Helpers.Enums;
using FreelanceTool.ViewModels;

namespace FreelanceTool.Helpers.CustomValidators
{
    public class EmployerConditionalRequiredAttribute : ValidationAttribute
    {
	    private readonly IEnumerable<Occupation> _occupationEmployerValues;

	    public EmployerConditionalRequiredAttribute()
	    {
			_occupationEmployerValues = new List<Occupation>
				{ Occupation.PartTime, Occupation.FullTime };
		}

	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
		    var applicationCreateVM = (ApplicationCreateViewModel) validationContext.ObjectInstance;
		    var currentOccupation = applicationCreateVM.Applicant.Occupation;
			if (_occupationEmployerValues.Contains(currentOccupation) &&
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
		    return "Employer field is required if occupation is: " +
		           $"{Occupation.PartTime} or {Occupation.FullTime}";
	    }
    }
}