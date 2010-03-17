using System;
using System.Collections.Generic;
using System.Linq;

using CodeQuery;

namespace MvbaMapper
{
	public class SimpleMapper
	{
		public static IEnumerable<SourceToDestination> GetAccessors(Type sourceType, Type destinationType)
		{
			var sourceProperties = sourceType
				.GetProperties()
				.ThatHaveAGetter()
				.ToDictionary(x => x.Name.ToLower());
			var destinationProperties = destinationType
				.GetProperties()
				.ThatHaveASetter()
				.Where(x => sourceProperties.ContainsKey(x.Name.ToLower()))
				.Where(x => x.PropertyType.IsAssignableFrom(sourceProperties[x.Name.ToLower()].PropertyType) ||
				            x.PropertyType.IsGenericAssignableFrom(sourceProperties[x.Name.ToLower()].PropertyType))
				.ToDictionary(x => x.Name.ToLower());
			var accessors = new List<SourceToDestination>();
			foreach (var destinationProperty in destinationProperties)
			{
				var sourceProperty = sourceProperties[destinationProperty.Key];

				var property = destinationProperty;
				var std = new SourceToDestination
					{
						GetValueFromSource = (source) => sourceProperty.GetValue(source, null),
						SetValueToDestination = (destination, value) => property.Value.SetValue(destination, value, null)
					};
				accessors.Add(std);
			}
			return accessors;
		}

		public void Map(object source, object destination)
		{
			if (source == null)
			{
				return;
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			foreach (var std in GetAccessors(source.GetType(), destination.GetType()))
			{
				var sourceValue = std.GetValueFromSource(source);
				std.SetValueToDestination(destination, sourceValue);
			}
		}
	}
}