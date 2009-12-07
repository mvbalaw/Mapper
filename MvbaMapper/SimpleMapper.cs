using System;
using System.Linq;

using CodeQuery;

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
			var sourceProperties = source.GetType()
				.GetProperties()
				.ThatHaveAGetter()
				.ToDictionary(x => x.Name);
			var destinationProperties = destination.GetType()
				.GetProperties()
				.ThatHaveASetter()
				.Where(x => sourceProperties.ContainsKey(x.Name))
				.Where(x => sourceProperties[x.Name].PropertyType.IsAssignableFrom(x.PropertyType))
				.ToDictionary(x => x.Name);
			foreach (var destinationProperty in destinationProperties.Values)
			{
				var sourceProperty = sourceProperties[destinationProperty.Name];
				var sourceValue = sourceProperty.GetValue(source, null);
				destinationProperty.SetValue(destination, sourceValue, null);
			}
		}
	}
}