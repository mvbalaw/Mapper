using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using CodeQuery;

using MvbaCore;

namespace MvbaMapper
{
	public class SimpleMapper
	{
		private static readonly LruCache<string, List<KeyValuePair<string, Action<object, object>>>> FrequentMaps = new LruCache<string, List<KeyValuePair<string, Action<object, object>>>>(50);

		public void Map(object source, object destination)
		{
			Map(source, destination, new Expression<Func<object, object>>[0]);
		}

		public void Map<TDestination>(object source, object destination,
		                              params Expression<Func<TDestination, object>>[] propertiesToIgnore)
		{
			if (source == null)
			{
				return;
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			var sourceType = source.GetType();
			var destinationType = destination.GetType();
			string key = sourceType.FullName + destinationType.FullName;
			var map = FrequentMaps[key];
			if (map == null)
			{
				map = Reflection.GetMatchingFieldsAndProperties(sourceType, destinationType)
					.Where(x => x.DestinationPropertyType.IsAssignableFrom(x.SourcePropertyType) ||
					            x.DestinationPropertyType.IsGenericAssignableFrom(x.SourcePropertyType))
					.Select(x => new KeyValuePair<string, Action<object, object>>(x.Name, (s, d) =>
						{
							var sourceValue = x.GetValueFromSource(s);
							x.SetValueToDestination(d, sourceValue);
						}))
					.ToList();
				FrequentMaps.Add(key, map);
			}

			var properties = propertiesToIgnore.Any() 
				? new HashSet<string>(new MapperUtilities().GetPropertyNames(propertiesToIgnore)) 
				: new HashSet<string>();
			foreach (var action in map
				.Where(x => !properties.Contains(x.Key)))
			{
				action.Value(source, destination);
			}
		}
	}
}