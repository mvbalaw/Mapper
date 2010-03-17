using System;
using System.Linq;

using CodeQuery;

using MvbaCore;

namespace MvbaMapper
{
	public class SimpleMapper
	{
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
			foreach (var std in Reflection.GetMatchingProperties(source.GetType(), destination.GetType())
				.Where(x => x.DestinationPropertyType.IsAssignableFrom(x.SourcePropertyType) ||
				            x.DestinationPropertyType.IsGenericAssignableFrom(x.SourcePropertyType)))
			{
				var sourceValue = std.GetValueFromSource(source);
				std.SetValueToDestination(destination, sourceValue);
			}
		}
	}
}