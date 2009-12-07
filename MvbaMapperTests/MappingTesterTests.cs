using FluentAssert;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class MappingTesterTests
	{
		[TestFixture]
		public class When_asked_for_the_input
		{
			private InputClass _source;
			private MappingTester<InputClass, OutputClass> tester;

			[SetUp]
			public void BeforeEachTest()
			{
				tester = new MappingTester<InputClass, OutputClass>();
				_source = tester.Source;
			}

			[Test]
			public void Should_initialize_bool_properties_with_non_default_values()
			{
				_source.BooleanProperty.ShouldNotBeEqualTo(default(bool));
			}

			[Test]
			public void Should_initialize_int_properties_with_non_default_values()
			{
				_source.IntegerProperty.ShouldNotBeEqualTo(default(int));
			}

			[Test]
			public void Should_initialize_string_properties_with_non_default_values()
			{
				_source.StringProperty.ShouldNotBeEqualTo(default(string));
				_source.StringProperty.ShouldNotBeEqualTo("");
			}

			[Test]
			public void Should_return_a_non_null_value()
			{
				_source.ShouldNotBeNull();
			}

			[Test]
			public void Should_return_the_same_object_on_subsequent_calls()
			{
				tester.Source.ShouldNotBeNull();
				ReferenceEquals(_source, tester.Source).ShouldBeTrue();
			}
		}

		[TestFixture]
		public class When_asked_to_verify_an_object_mapping
		{
			private MappingTester<InputClass, OutputClass> tester;
			private OutputClass _expected;

			[SetUp]
			public void BeforeEachTest()
			{
				tester = new MappingTester<InputClass, OutputClass>();
				var source = tester.Source;
				_expected = new OutputClass
				{
					BooleanProperty = source.BooleanProperty,
					IntegerProperty = source.IntegerProperty,
					StringProperty = source.StringProperty
				};
			}

			[Test, ExpectedException(typeof(AssertionException), UserMessage = "Property IntegerProperty does not match")]
			public void Should_detect_int_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = _expected.BooleanProperty,
					StringProperty = _expected.StringProperty,
					IntegerProperty = _expected.IntegerProperty - 1
				};
				tester.Verify(actual, _expected);
			}

			[Test, ExpectedException(typeof(AssertionException), UserMessage = "Property BooleanProperty does not match")]
			public void Should_detect_bool_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = !_expected.BooleanProperty,
					StringProperty = _expected.StringProperty,
					IntegerProperty = _expected.IntegerProperty
				};
				tester.Verify(actual, _expected);
			}

			[Test, ExpectedException(typeof(AssertionException), UserMessage = "Property StringProperty does not match")]
			public void Should_detect_string_property_differences()
			{
				var actual = new OutputClass
				{
					BooleanProperty = _expected.BooleanProperty,
					StringProperty = _expected.StringProperty.ToUpper(),
					IntegerProperty = _expected.IntegerProperty
				};
				tester.Verify(actual, _expected);
			}
		}
	}

	public class OutputClass
	{
		public bool BooleanProperty { get; set; }
		public int IntegerProperty { get; set; }
		public string StringProperty { get; set; }
	}

	public class InputClass
	{
		public bool BooleanProperty { get; set; }
		public int IntegerProperty { get; set; }
		public string StringProperty { get; set; }
	}
}