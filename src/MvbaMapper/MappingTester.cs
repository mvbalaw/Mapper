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
			var thatHaveAGetter = expected.GetType()
				.GetProperties()
				.ThatHaveAGetter();
			var thatHaveASetter = thatHaveAGetter
				.ThatHaveASetter();
			var thatAreEnumerable = thatHaveAGetter
				.Where(x => typeof (IEnumerable).IsAssignableFrom(x.PropertyType));
			var destinationProperties = thatHaveASetter.Concat(thatAreEnumerable).Distinct()
				.ToDictionary(x => x.Name, x => x);

			foreach (var propertyInfo in destinationProperties.Values)
			{
				try
				{
					var expectedValue = propertyInfo.GetValue(expected, null);
					var actualValue = propertyInfo.GetValue(actual, null);

					if (expectedValue == null && actualValue == null)
					{
						continue;
					}
					if (expectedValue == null || actualValue == null)
					{
						notification.Add(Notification.WarningFor(propertyInfo.Name));
						continue;
					}

					if (expectedValue.Equals(actualValue))
					{
						continue;
					}

					if (typeof (IEnumerable).IsAssignableFrom(expectedValue.GetType()) &&
					    typeof (IEnumerable).IsAssignableFrom(actualValue.GetType()))
					{
						var comparer = new EnumerableComparer();
						if (!comparer.HaveSameContents((IEnumerable) expectedValue, (IEnumerable) actualValue))
						{
							notification.Add(Notification.WarningFor(propertyInfo.Name));
						}
						continue;
					}
					if (!expectedValue.Equals(actualValue))
					{
						notification.Add(Notification.WarningFor(propertyInfo.Name));
					}
				}
				catch (Exception e)
				{
					throw new ArgumentException("unexpected exception testing property " + propertyInfo.Name, e);
				}
			}
			return notification;
		}
	}
}