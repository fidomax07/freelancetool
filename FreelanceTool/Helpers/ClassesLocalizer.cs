using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace FreelanceTool.Helpers
{
	public class ClassesLocalizer
	{
		private readonly IStringLocalizerFactory _factory;
		private IStringLocalizer _localizer;

		public ClassesLocalizer(IStringLocalizerFactory factory)
		{
			_factory = factory;
		}

		public LocalizedString Localize<T>(string propertyName)
			where T : class
		{
			var classType = typeof(T);

			_localizer = _factory.Create(classType);

			var content = classType
				.GetProperty(propertyName)?
				.GetCustomAttribute<DisplayAttribute>()?
				.Name;

			return _localizer[content ?? propertyName];
		}
	}
}