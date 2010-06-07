using System;
using System.Linq;
using System.Linq.Expressions;
using CodeQuery;
using MvbaCore;

namespace MvbaMapper
{
	public class SimpleMapper
	{
		public void Map(object source, object destination)
		{
			Map(source, destination, new Expression<Func<object, object>>[0]);
		}

		public void Map<TDestination>(object source, object destination,
		                              Expression<Func<TDestination, object>>[] propertiesToIgnore)
		{
			var properties = new MapperUtilities().GetPropertyNames(propertiesToIgnore);
			if (source == null)
			{
				return;
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			foreach (var std in Reflection.GetMatchingProperties(source.GetType(), destination.GetType())
				.Where(x => !properties.Contains(x.Name))
				.Where(x => x.DestinationPropertyType.IsAssignableFrom(x.SourcePropertyType) ||
				            x.DestinationPropertyType.IsGenericAssignableFrom(x.SourcePropertyType)))
			{
				var sourceValue = std.GetValueFromSource(source);
				std.SetValueToDestination(destination, sourceValue);
			}
		}
	}
}