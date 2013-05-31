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

using MvbaCore;
using MvbaCore.CodeQuery;
using MvbaCore.Collections;

namespace MvbaMapper
{
	public class SimpleMapper
	{
		private static readonly LruCache<string, List<KeyValuePair<string, Action<object, object>>>> FrequentMaps = new LruCache<string, List<KeyValuePair<string, Action<object, object>>>>(50);

		public SimpleMapperParameters Except<TDestination>(Expression<Func<TDestination, object>> destinationProperty)
		{
			return new SimpleMapperParameters().Except(destinationProperty);
		}

		public SimpleMapperParameters Except<TDestination>(params Expression<Func<TDestination, object>>[] destinationProperties)
		{
			return new SimpleMapperParameters().Except(destinationProperties);
		}

		public SimpleMapperParameters Link<TSource, TDestination>(Expression<Func<TSource, object>> sourceProperty, Expression<Func<TDestination, object>> destinationProperty)
		{
			return new SimpleMapperParameters().Link(sourceProperty, destinationProperty);
		}

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
		                 ICollection<string> propertiesToIgnore,
		                 ICollection<ITypeConverter> customConverters)
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
			if (IsProxyClass(sourceType))
			{
				sourceType = sourceType.BaseType;
			}
			string key = sourceType.FullName + destinationType.FullName;
			var map = FrequentMaps[key];
			if (map == null || customDestinationPropertyNameToSourcePropertyNameMap.Count > 0)
			{
				map = Reflection.GetMatchingFieldsAndProperties(sourceType, destinationType, customDestinationPropertyNameToSourcePropertyNameMap)
					.Where(x => x.DestinationPropertyType.IsAssignableFrom(x.SourcePropertyType) ||
					            x.DestinationPropertyType.IsGenericAssignableFrom(x.SourcePropertyType) ||
					            customConverters.Any(y => y.CanConvert(x.SourcePropertyType, x.DestinationPropertyType)))
					.Select(x => new KeyValuePair<string, Action<object, object>>(x.Name, (s, d) =>
						{
							var sourceValue = x.GetValueFromSource(s);
							var converter = customConverters.FirstOrDefault(y => y.CanConvert(x.SourcePropertyType, x.DestinationPropertyType));
							if (converter != null)
							{
								sourceValue = converter.Convert(sourceValue);
							}
							x.SetValueToDestination(d, sourceValue);
						}))
					.ToList();
				if (customDestinationPropertyNameToSourcePropertyNameMap.Count == 0 &&
				    customConverters.Count == 0)
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

		public bool IsProxyClass(Type sourceType)
		{
			return sourceType.GetInterfaces().Any(x => x.FullName == "NHibernate.Proxy.DynamicProxy.IProxy");
		}

		public class SimpleMapperParameters
		{
			private readonly IList<ITypeConverter> _customConverters = new List<ITypeConverter>();
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

			public SimpleMapperParameters AddCustomConverter(Func<Type, Type, bool> canConvert, Func<object, object> convert)
			{
				return AddCustomConverter(new TypeConverter
				{
					CanConvert = canConvert,
					Convert = convert
				});
			}

			public SimpleMapperParameters AddCustomConverter(ITypeConverter typeConverter)
			{
				_customConverters.Add(typeConverter);
				return this;
			}

			public SimpleMapperParameters AddCustomConverters(params ITypeConverter[] typeConverters)
			{
				typeConverters.ForEach(_customConverters.Add);
				return this;
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
				string sourcePropertyName = Reflection.GetPropertyName(sourceProperty);
				string destinationPropertyName = Reflection.GetPropertyName(destinationProperty);
				_customDestinationPropertyNameToSourcePropertyNameMap.Add(destinationPropertyName, sourcePropertyName);
				return this;
			}

			public void Map(object source, object destination)
			{
				_mapper.Map(source, destination, _customDestinationPropertyNameToSourcePropertyNameMap, _exceptions, _customConverters);
			}
		}
	}
}