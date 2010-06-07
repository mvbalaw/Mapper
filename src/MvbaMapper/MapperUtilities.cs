using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvbaCore;

namespace MvbaMapper
{
	public class MapperUtilities
	{
		public List<string> GetPropertyNames<TDestination>(
			IEnumerable<Expression<Func<TDestination, object>>> propertiesToIgnore)
		{
			var properties = new List<string>();
			propertiesToIgnore.ForEach(x => properties.Add(Reflection.GetPropertyName(x)));
			return properties;
		}
		
	}
}