using System;
using System.Collections.Generic;
using System.Linq;

using CodeQuery;

namespace MvbaMapper
{
	public class SimpleMapper
	{
		public static IEnumerable<PropertyMappingInfo> GetAccessors(Type sourceType, Type destinationType)
		{
			var sourceProperties = sourceType
				.GetProperties()
				.ThatHaveAGetter()
				.ToDictionary(x => x.Name.ToLower());
			var destinationProperties = destinationType
				.GetProperties()
				.ThatHaveASetter()
				.Where(x => sourceProperties.ContainsKey(x.Name.ToLower()))
				.ToDictionary(x => x.Name.ToLower());
			var accessors = new List<PropertyMappingInfo>();
			foreach (var destinationProperty in destinationProperties)
			{
				var sourceProperty = sourceProperties[destinationProperty.Key];

				var property = destinationProperty;
				var propertyMappingInfo = new PropertyMappingInfo
					{
						Name = destinationProperty.Value.Name,
						SourcePropertyType = sourceProperty.PropertyType,
						DestinationPropertyType = destinationProperty.Value.PropertyType,
						GetValueFromSource = source => sourceProperty.GetValue(source, null),
						SetValueToDestination = (destination, value) => property.Value.SetValue(destination, value, null)
					};
				accessors.Add(propertyMappingInfo);
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
			foreach (var std in GetAccessors(source.GetType(), destination.GetType())
				.Where(x => x.DestinationPropertyType.IsAssignableFrom(x.SourcePropertyType) ||
				            x.DestinationPropertyType.IsGenericAssignableFrom(x.SourcePropertyType)))
			{
				var sourceValue = std.GetValueFromSource(source);
				std.SetValueToDestination(destination, sourceValue);
			}
		}
	}
}