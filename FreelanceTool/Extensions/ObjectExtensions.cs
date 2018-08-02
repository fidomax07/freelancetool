using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
	public static class ObjectExtensions
	{
		public static IEnumerable<PropertyInfo> GetProperties(this Object objectInstance)
		{
			return objectInstance
				.GetType()
				.GetProperties();
		}

		public static HashSet<string> GetPropertiesNames(this Object objectInstance)
		{
			return objectInstance
				.GetProperties()
				.Select(p => p.Name)
				.ToHashSet();
		}
	}
}