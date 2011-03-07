using System;
using System.Collections;
using System.Linq;

using CodeQuery;

using MvbaCore;

namespace MvbaMapper
{
	public class MappingTester<TDestination>
	{
		public Notification Verify(TDestination actual, TDestination expected)
		{
			var notification = new Notification();
			var properties = expected.GetType()
				.GetProperties()
				.ThatHaveAGetter()
				.ThatHaveASetter()
				.Select(x =>
				        	{
				        		Func<TDestination, object> getter = instance => x.GetValue(instance, null);
				        		return new
				        			{
				        				x.Name,
				        				GetValue = getter,
				        				ReturnType = x.PropertyType
				        			};
				        	});
			var fields = expected.GetType()
				.GetFields()
				.Select(x =>
				        	{
				        		Func<TDestination, object> getter = instance => x.GetValue(instance);
				        		return new
				        			{
				        				x.Name,
				        				GetValue = getter,
				        				ReturnType = x.FieldType
				        			};
				        	});

			var lookup = properties.Concat(fields).ToDictionary(x => x.Name, x => x);
			var comparer = new EnumerableComparer();
			foreach (var propertyOrField in lookup.Values)
			{
				try
				{
					var expectedValue = propertyOrField.GetValue(expected);
					var actualValue = propertyOrField.GetValue(actual);

					if (expectedValue == null && actualValue == null)
					{
						continue;
					}
					if (expectedValue == null || actualValue == null)
					{
						notification.Add(Notification.WarningFor(propertyOrField.Name));
						continue;
					}

					if (expectedValue.Equals(actualValue))
					{
						continue;
					}

					if (typeof(IEnumerable).IsAssignableFrom(expectedValue.GetType()) &&
					    typeof(IEnumerable).IsAssignableFrom(actualValue.GetType()))
					{
						if (!comparer.HaveSameContents((IEnumerable)expectedValue, (IEnumerable)actualValue))
						{
							notification.Add(Notification.WarningFor(propertyOrField.Name));
						}
						continue;
					}
					if (!expectedValue.Equals(actualValue))
					{
						notification.Add(Notification.WarningFor(propertyOrField.Name));
					}
				}
				catch (Exception e)
				{
					throw new ArgumentException("unexpected exception testing property " + propertyOrField.Name, e);
				}
			}
			return notification;
		}
	}
}