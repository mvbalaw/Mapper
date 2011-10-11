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
		public static MapRequest Map<TItem>(this TItem sourceOrDestination)
		{
			return new MapRequest(sourceOrDestination);
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
				.To<TDestination>();
		}

		public class MapRequest
		{
			private readonly SimpleMapper.SimpleMapperParameters _mapper = new SimpleMapper.SimpleMapperParameters();
			private readonly object _sourceOrDestination;

			public MapRequest(object sourceOrDestination)
			{
				_sourceOrDestination = sourceOrDestination;
			}

			public MapRequest Except<TDestination>(Expression<Func<TDestination, object>> destination)
			{
				_mapper.Except(destination);
				return this;
			}

			public MapRequest Except<TDestination>(params Expression<Func<TDestination, object>>[] destinationProperties)
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

			public MapRequest WithLink<TSource, TDestination>(Expression<Func<TSource, object>> source, Expression<Func<TDestination, object>> destination)
			{
				_mapper.Link(source, destination);
				return this;
			}
		}
	}
}