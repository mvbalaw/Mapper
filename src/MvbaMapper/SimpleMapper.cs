//  * **************************************************************************
//  * Copyright (c) McCreary, Veselka, Bragg & Allen, P.C.
//  * This source code is subject to terms and conditions of the MIT License.
//  * A copy of the license can be found in the License.txt file
//  * at the root of this distribution. 
//  * By using this source code in any fashion, you are agreeing to be bound by 
//  * the terms of the MIT License.
//  * You must not remove this notice from this software.
//  * **************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
			new SimpleMapperParameters(this)
				.Map(source, destination);
		}

		public void Map<TDestination>(object source, object destination,
		                              params Expression<Func<TDestination, object>>[] propertiesToIgnore)
		{
			new SimpleMapperParameters(this)
				.Except(propertiesToIgnore)
				.Map(source, destination);
		}

		private void Map(object source,
		                 object destination,
		                 StringDictionary customDestinationPropertyNameToSourcePropertyNameMap,
		                 ICollection<string> propertiesToIgnore)
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
			if (map == null || customDestinationPropertyNameToSourcePropertyNameMap.Count > 0)
			{
				map = Reflection.GetMatchingFieldsAndProperties(sourceType, destinationType, customDestinationPropertyNameToSourcePropertyNameMap)
					.Where(x => x.DestinationPropertyType.IsAssignableFrom(x.SourcePropertyType) ||
					            x.DestinationPropertyType.IsGenericAssignableFrom(x.SourcePropertyType))
					.Select(x => new KeyValuePair<string, Action<object, object>>(x.Name, (s, d) =>
						{
							var sourceValue = x.GetValueFromSource(s);
							x.SetValueToDestination(d, sourceValue);
						}))
					.ToList();
				if (customDestinationPropertyNameToSourcePropertyNameMap.Count == 0)
				{
					FrequentMaps.Add(key, map);
				}
			}

			foreach (var action in map
				.Where(x => !propertiesToIgnore.Contains(x.Key)))
			{
				action.Value(source, destination);
			}
		}

		public class SimpleMapperParameters
		{
			private readonly StringDictionary _customDestinationPropertyNameToSourcePropertyNameMap = new StringDictionary();
			private readonly HashSet<string> _exceptions = new HashSet<string>();
			private readonly SimpleMapper _mapper;

			internal SimpleMapperParameters()
				: this(new SimpleMapper())
			{
			}

			internal SimpleMapperParameters(SimpleMapper mapper)
			{
				_mapper = mapper;
			}

			public SimpleMapperParameters Except<TDestination>(Expression<Func<TDestination, object>> destinationProperty)
			{
				string destinationPropertyName = Reflection.GetPropertyName(destinationProperty);
				_exceptions.Add(destinationPropertyName);
				return this;
			}

			public SimpleMapperParameters Except<TDestination>(params Expression<Func<TDestination, object>>[] destinationProperties)
			{
				foreach (var destinationProperty in destinationProperties)
				{
					string destinationPropertyName = Reflection.GetPropertyName(destinationProperty);
					_exceptions.Add(destinationPropertyName);
				}
				return this;
			}

			public SimpleMapperParameters Link<TSource, TDestination>(Expression<Func<TSource, object>> sourceProperty, Expression<Func<TDestination, object>> destinationProperty)
			{
				string sourcePropertyName = Reflection.GetPropertyName(sourceProperty).ToLower();
				string destinationPropertyName = Reflection.GetPropertyName(destinationProperty).ToLower();
				_customDestinationPropertyNameToSourcePropertyNameMap.Add(destinationPropertyName, sourcePropertyName);
				return this;
			}

			public void Map(object source, object destination)
			{
				_mapper.Map(source, destination, _customDestinationPropertyNameToSourcePropertyNameMap, _exceptions);
			}
		}
	}
}