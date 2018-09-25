using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FreelanceTool.Models
{
	public abstract class ApplicationModel
	{
		/// <summary>
		/// Retrieves the error message of the given attribute for the given property
		/// of the <see cref="T:FreelanceTool.Models.ApplicationModel"/>.
		/// </summary>
		/// <typeparam name="T">The type of attribute to search for.</typeparam>
		/// <param name="propertyName">The property name to search for.</param>
		/// <returns>The error message or null if message cannot be retrieved.</returns>
		public string GetAttributeErrorMessage<T>(string propertyName)
			where T : ValidationAttribute
		{
			return GetType()
				.GetProperty(propertyName)?
				.GetCustomAttribute<T>()?
				.ErrorMessage;
		}
	}
}