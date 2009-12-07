using System.Collections.Generic;
using System.Linq;

using CodeQuery;

using FluentAssert;

namespace MvbaMapperTests
{
	public class MappingTester<TSource, TDestination>
		where TSource : new()
	{
		private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
		private readonly TSource _source;

		public MappingTester()
		{
			_source = new TSource();
			Populate(_source);
		}

		public TSource Source
		{
			get { return _source; }
		}

		private void Populate(TSource source)
		{
			foreach (var propertyInfo in typeof(TSource)
				.GetProperties()
				.ThatHaveAGetter()
				.ThatHaveASetter())
			{
				var value = RandomValueType.GetFor(propertyInfo.PropertyType.FullName).CreateRandomValue();
				_propertyValues.Add(propertyInfo.Name, value);
				propertyInfo.SetValue(source, value, null);
			}
		}

		public void Verify(TDestination actual, TDestination expected)
		{
			var destinationProperties = typeof(TDestination)
				.GetProperties()
				.ThatHaveASetter()
				.ThatHaveAGetter()
				.ToDictionary(x => x.Name, x => x);

			foreach (var propertyInfo in destinationProperties.Values)
			{
				var expectedValue = propertyInfo.GetValue(expected, null);
				var actualValue = propertyInfo.GetValue(actual, null);
				actualValue.ShouldBeEqualTo(expectedValue, "Property " + propertyInfo.Name + " does not match");
			}
		}
	}
}