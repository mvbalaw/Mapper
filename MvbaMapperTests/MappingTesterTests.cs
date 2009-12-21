using System.Collections.Generic;

using FluentAssert;

using MvbaMapper;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class MappingTesterTests
	{
		[TestFixture]
		public class When_asked_to_verify_an_object_mapping
		{
			private OutputClass _expected;
			private MappingTester<OutputClass> tester;

			[SetUp]
			public void BeforeEachTest()
			{
				tester = new MappingTester<OutputClass>();
				var source = new ClassFiller<InputClass>().Source;
				_expected = new OutputClass
				{
					BooleanProperty = source.BooleanProperty,
					IntegerProperty = source.IntegerProperty,
					StringProperty = source.StringProperty,
					DecimalProperty = source.DecimalProperty,
					DateTimeProperty = source.DateTimeProperty
				};
			}

			[Test]
			public void Should_detect_bool_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = !_expected.BooleanProperty,
					StringProperty = _expected.StringProperty,
					DecimalProperty = _expected.DecimalProperty,
					DateTimeProperty = _expected.DateTimeProperty,
					IntegerProperty = _expected.IntegerProperty
				};
				var notification = tester.Verify(actual, _expected);
				notification.IsValid.ShouldBeFalse();
				notification.ToString().ShouldBeEqualTo("BooleanProperty");
			}

			[Test]
			public void Should_detect_DateTime_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = _expected.BooleanProperty,
					StringProperty = _expected.StringProperty,
					DecimalProperty = _expected.DecimalProperty,
					DateTimeProperty = _expected.DateTimeProperty.AddDays(1),
					IntegerProperty = _expected.IntegerProperty
				};
				var notification = tester.Verify(actual, _expected);
				notification.IsValid.ShouldBeFalse();
				notification.ToString().ShouldBeEqualTo("DateTimeProperty");
			}

			[Test]
			public void Should_detect_decimal_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = _expected.BooleanProperty,
					StringProperty = _expected.StringProperty,
					DecimalProperty = _expected.DecimalProperty - 1,
					DateTimeProperty = _expected.DateTimeProperty,
					IntegerProperty = _expected.IntegerProperty
				};
				var notification = tester.Verify(actual, _expected);
				notification.IsValid.ShouldBeFalse();
				notification.ToString().ShouldBeEqualTo("DecimalProperty");
			}

			[Test]
			public void Should_detect_int_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = _expected.BooleanProperty,
					StringProperty = _expected.StringProperty,
					DecimalProperty = _expected.DecimalProperty,
					DateTimeProperty = _expected.DateTimeProperty,
					IntegerProperty = _expected.IntegerProperty - 1
				};
				var notification = tester.Verify(actual, _expected);
				notification.IsValid.ShouldBeFalse();
				notification.ToString().ShouldBeEqualTo("IntegerProperty");
			}

			[Test]
			public void Should_detect_string_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = _expected.BooleanProperty,
					StringProperty = _expected.StringProperty.ToUpper(),
					DecimalProperty = _expected.DecimalProperty,
					DateTimeProperty = _expected.DateTimeProperty,
					IntegerProperty = _expected.IntegerProperty
				};
				var notification = tester.Verify(actual, _expected);
				notification.IsValid.ShouldBeFalse();
				notification.ToString().ShouldBeEqualTo("StringProperty");
			}

			[Test]
			public void Should_list_content_differences()
			{
				var actual = new OutputListClass
				{
					Items = new List<int>
					{
						4,
						5,
						6
					}
				};
				var expected = new OutputListClass
				{
					Items = new List<int>
					{
						4,
						5,
						6,
						7
					}
				};
				var notification = new MappingTester<OutputListClass>().Verify(actual, expected);
				notification.IsValid.ShouldBeFalse();
				notification.ToString().ShouldBeEqualTo("Items");
			}
		}
	}
}