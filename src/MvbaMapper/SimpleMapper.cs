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
				.ToDictionary(x => x.Name.ToLower());
			var destinationProperties = destination.GetType()
				.GetProperties()
				.ThatHaveASetter()
				.Where(x => sourceProperties.ContainsKey(x.Name.ToLower()))
				.Where(x => x.PropertyType.IsAssignableFrom(sourceProperties[x.Name.ToLower()].PropertyType) ||
							x.PropertyType.IsGenericAssignableFrom(sourceProperties[x.Name.ToLower()].PropertyType))
				.ToDictionary(x => x.Name.ToLower());
			foreach (var destinationProperty in destinationProperties)
			{
				var sourceProperty = sourceProperties[destinationProperty.Key];
				var sourceValue = sourceProperty.GetValue(source, null);
				destinationProperty.Value.SetValue(destination, sourceValue, null);
			}
		}
	}
}