using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FreelanceTool.Helpers;

namespace System
{
	public static class EnumExtensions
	{
		public static string GetName(this Enum enumValue)
		{
			return Enum.GetName(enumValue.GetType(), enumValue);
		}

		public static string GetDescription(this Enum enumValue)
		{
			var field = enumValue
				.GetType()
				.GetField(enumValue.GetName());

			var descriptionAttribute = field?
				.GetCustomAttributes(typeof(DescriptionAttribute), false)?
				.FirstOrDefault() as DescriptionAttribute;

			return descriptionAttribute?.Description ?? enumValue.GetName();
		}

		public static string GetDisplayName(this Enum enumValue)
		{
			var displayAttribute = enumValue
				.GetType()
				.GetField(enumValue.GetName())?
				.GetCustomAttributes(typeof(DisplayAttribute), false)?
				.FirstOrDefault() as DisplayAttribute;

			return displayAttribute?.Name ?? enumValue.GetName();
		}
	}
}
