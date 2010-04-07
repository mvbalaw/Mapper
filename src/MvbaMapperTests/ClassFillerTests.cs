using FluentAssert;

using MvbaMapper;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class ClassFillerTests
	{
		[TestFixture]
		public class When_asked_for_the_Source
		{
			private InputClass _source;
			private ClassFiller<InputClass> tester;

			[SetUp]
			public void BeforeEachTest()
			{
				tester = new ClassFiller<InputClass>();
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

	}
}