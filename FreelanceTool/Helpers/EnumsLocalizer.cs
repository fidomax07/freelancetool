using System;
using Microsoft.Extensions.Localization;

namespace FreelanceTool.Helpers
{
	public class EnumsLocalizer
	{
		private readonly IStringLocalizerFactory _factory;
		private IStringLocalizer _localizer;

		public EnumsLocalizer(IStringLocalizerFactory factory)
		{
			_factory = factory;
		}

		public LocalizedString Localize<T>(T enumValue)
			where T : IConvertible
		{
			var enumType = typeof(T);

			if (!enumType.IsEnum)
				throw new ArgumentException("T must be an enumerated type");

			_localizer = _factory.Create(enumType);

			var content = Enum.GetName(enumType, enumValue);

			return _localizer[content];
		}
	}
}