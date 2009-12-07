using System.Linq;

using CodeQuery;

namespace MvbaMapper
{
	public class SimpleMapper<TSource, TDestination>
	{
		public void Map(TSource source, TDestination destination)
		{
			var sourceProperties = typeof(TSource)
				.GetProperties()
				.ThatHaveAGetter()
				.ToDictionary(x => x.Name);
			var destinationProperties = typeof(TDestination)
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