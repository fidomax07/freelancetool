using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace FreelanceTool.Helpers
{
	public class AppLocalizer
	{
		private readonly IStringLocalizerFactory _factory;
		private IStringLocalizer _localizer;

		public AppLocalizer(IStringLocalizerFactory factory)
		{
			_factory = factory;
		}

		public LocalizedString LocalizeEnum(Enum enumValue)
		{
			_localizer = _factory.Create(enumValue.GetType());

			return _localizer[enumValue.GetDisplayName()];
		}

		public LocalizedString LocalizeClassMember<T>(string propertyName)
			where T : class
		{
			var classType = typeof(T);

			_localizer = _factory.Create(classType);

			var displayAttribute = classType
				.GetProperty(propertyName)?
				.GetCustomAttribute<DisplayAttribute>();

			return _localizer[displayAttribute?.Name ?? propertyName];
		}
	}
}