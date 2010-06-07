using System;
using System.Linq.Expressions;

namespace MvbaMapper
{
	public static class TExtensions
	{
		public static void MapFrom<TDestination>(this TDestination destination, object source,
		                                         params Expression<Func<TDestination, object>>[] propertiesToIgnore)
		{
			if (propertiesToIgnore.Length == 0)
			{
				new SimpleMapper().Map(source, destination);
				return;
			}
			new SimpleMapper().Map(source, destination, propertiesToIgnore);
		}

		public static void MapFrom(this object destination, object source)
		{
			new SimpleMapper().Map(source, destination);
		}

		public static TDestination MapTo<TDestination>(this object source)
			where TDestination : class, new()
		{
			return MapTo(source, new Expression<Func<TDestination, object>>[0]);
		}

		public static TDestination MapTo<TDestination>(this object source,
		                                               params Expression<Func<TDestination, object>>[] propertiesToIgnore)
			where TDestination : class, new()
		{
			var destination = new TDestination();
			destination.MapFrom(source, propertiesToIgnore);
			return destination;
		}
	}
}