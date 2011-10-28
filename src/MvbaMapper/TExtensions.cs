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
using System.Linq.Expressions;

namespace MvbaMapper
{
	public static class TExtensions
	{
		public static MapRequest<TItem> Map<TItem>(this TItem sourceOrDestination)
		{
			return new MapRequest<TItem>(sourceOrDestination);
		}

		public static void MapFrom<TDestination>(this TDestination destination, object source,
		                                         params Expression<Func<TDestination, object>>[] propertiesToIgnore)
		{
			Map(destination)
				.Except(propertiesToIgnore)
				.From(source);
		}

		public static void MapFrom(this object destination, object source)
		{
			Map(destination).From(source);
		}

		public static TDestination MapTo<TDestination>(this object source)
			where TDestination : class, new()
		{
			return Map(source).To<TDestination>();
		}

		public static TDestination MapTo<TDestination>(this object source,
		                                               params Expression<Func<TDestination, object>>[] propertiesToIgnore)
			where TDestination : class, new()
		{
			return Map(source)
				.Except(propertiesToIgnore)
				.To();
		}

		public class MapFromRequest<TSource, TDestination>
			where TDestination : new()
		{
			private readonly SimpleMapper.SimpleMapperParameters _mapper;
			private readonly TSource _source;

			public MapFromRequest(SimpleMapper.SimpleMapperParameters mapper, TSource source)
			{
				_mapper = mapper;
				_source = source;
			}

			public MapFromRequest<TSource, TDestination> Except(Expression<Func<TDestination, object>> destination)
			{
				_mapper.Except(destination);
				return this;
			}

			public MapFromRequest<TSource, TDestination> Except(params Expression<Func<TDestination, object>>[] destinationProperties)
			{
				_mapper.Except(destinationProperties);
				return this;
			}

			public TDestination To()
			{
				var destination = new TDestination();
				_mapper.Map(_source, destination);
				return destination;
			}

			public void To(object destination)
			{
				_mapper.Map(_source, destination);
			}

			public MapFromRequest<TSource, TDestination> WithCustomConversion(Func<Type, Type, bool> canConvert, Func<object, object> convert)
			{
				_mapper.AddCustomConverter(canConvert, convert);
				return this;
			}

			public MapFromRequest<TSource, TDestination> WithCustomConverter(ITypeConverter typeConverter)
			{
				_mapper.AddCustomConverter(typeConverter);
				return this;
			}

			public MapFromRequest<TSource, TDestination> WithCustomConverters(params ITypeConverter[] typeConverters)
			{
				_mapper.AddCustomConverters(typeConverters);
				return this;
			}

			public MapFromRequest<TSource, TDestination> WithLink(Expression<Func<TSource, object>> source, Expression<Func<TDestination, object>> destination)
			{
				_mapper.Link(source, destination);
				return this;
			}
		}

		public class MapRequest<TItem>
		{
			private readonly SimpleMapper.SimpleMapperParameters _mapper = new SimpleMapper.SimpleMapperParameters();
			private readonly TItem _sourceOrDestination;

			public MapRequest(TItem sourceOrDestination)
			{
				_sourceOrDestination = sourceOrDestination;
			}

			public MapFromRequest<TItem, TDestination> Except<TDestination>(Expression<Func<TDestination, object>> destination)
				where TDestination : new()
			{
				return new MapFromRequest<TItem, TDestination>(_mapper, _sourceOrDestination)
					.Except(destination);
			}

			public MapFromRequest<TItem, TDestination> Except<TDestination>(params Expression<Func<TDestination, object>>[] destinationProperties)
				where TDestination : new()
			{
				return new MapFromRequest<TItem, TDestination>(_mapper, _sourceOrDestination)
					.Except(destinationProperties);
			}

			public MapRequest<TItem> Except(params Expression<Func<TItem, object>>[] destinationProperties)
			{
				_mapper.Except(destinationProperties);
				return this;
			}

			public void From(object source)
			{
				_mapper.Map(source, _sourceOrDestination);
			}

			public TDestination To<TDestination>() where TDestination : new()
			{
				var destination = new TDestination();
				_mapper.Map(_sourceOrDestination, destination);
				return destination;
			}

			public void To(object destination)
			{
				_mapper.Map(_sourceOrDestination, destination);
			}

			public MapRequest<TItem> WithCustomConversion(Func<Type, Type, bool> canConvert, Func<object, object> convert)
			{
				_mapper.AddCustomConverter(canConvert, convert);
				return this;
			}

			public MapRequest<TItem> WithCustomConverter(ITypeConverter typeConverter)
			{
				_mapper.AddCustomConverter(typeConverter);
				return this;
			}

			public MapRequest<TItem> WithCustomConverters(params ITypeConverter[] typeConverters)
			{
				_mapper.AddCustomConverters(typeConverters);
				return this;
			}

			public MapToRequest<TSource, TItem> WithLink<TSource>(Expression<Func<TSource, object>> source, Expression<Func<TItem, object>> destination)
			{
				return new MapToRequest<TSource, TItem>(_mapper, _sourceOrDestination)
					.WithLink(source, destination);
			}

			public MapFromRequest<TItem, TDestination> WithLink<TDestination>(Expression<Func<TItem, object>> source, Expression<Func<TDestination, object>> destination)
				where TDestination : new()
			{
				return new MapFromRequest<TItem, TDestination>(_mapper, _sourceOrDestination)
					.WithLink(source, destination);
			}
		}

		public class MapToRequest<TSource, TDestination>
		{
			private readonly TDestination _destination;
			private readonly SimpleMapper.SimpleMapperParameters _mapper;

			public MapToRequest(SimpleMapper.SimpleMapperParameters mapper, TDestination destination)
			{
				_mapper = mapper;
				_destination = destination;
			}

			public void From(object source)
			{
				_mapper.Map(source, _destination);
			}

			public MapToRequest<TSource, TDestination> WithCustomConversion(Func<Type, Type, bool> canConvert, Func<object, object> convert)
			{
				_mapper.AddCustomConverter(canConvert, convert);
				return this;
			}

			public MapToRequest<TSource, TDestination> WithCustomConverter(ITypeConverter typeConverter)
			{
				_mapper.AddCustomConverter(typeConverter);
				return this;
			}

			public MapToRequest<TSource, TDestination> WithCustomConverters(params ITypeConverter[] typeConverters)
			{
				_mapper.AddCustomConverters(typeConverters);
				return this;
			}

			public MapToRequest<TSource, TDestination> WithLink(Expression<Func<TSource, object>> source, Expression<Func<TDestination, object>> destination)
			{
				_mapper.Link(source, destination);
				return this;
			}
		}
	}
}