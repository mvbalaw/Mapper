using FluentAssert;

using MvbaMapper;

using MvbaMapperTests.TestClasses;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class TExtensionsTests
	{
		[TestFixture]
		public class When_asked_to_map_from_another_class
		{
			private InputClass _source;

			[SetUp]
			public void BeforeEachTest()
			{
				_source = new ClassFiller<InputClass>().Source;
			}

			[Test]
			public void Given_a_source_object()
			{
				Test.Given(new OutputClass())
					.When(MapFrom_is_called)
					.Should(Map_the_property_values)
					.Verify();
			}

			private void Map_the_property_values(OutputClass destination)
			{
				var tester = new MappingTester<OutputClass>();
				var expected = new OutputClass
					{
						BooleanProperty = _source.BooleanProperty,
						IntegerProperty = _source.IntegerProperty,
						StringProperty = _source.StringProperty,
						DecimalProperty = _source.DecimalProperty,
						DateTimeProperty = _source.DateTimeProperty,
						DateTimeToNullable = _source.DateTimeToNullable
					};
				var result = tester.Verify(destination, expected);
				result.IsValid.ShouldBeTrue();
				result.ToString().ShouldBeEqualTo("");
			}

			private void MapFrom_is_called(OutputClass destination)
			{
				destination.MapFrom(_source);
			}
		}

		[TestFixture]
		public class When_asked_to_map_to_a_specified_type
		{
			private OutputClass _destination;
			private OutputClass _expected;

			[Test]
			public void Should_populate_properties()
			{
				Test.Given(new ClassFiller<InputClass>().Source)
					.When(MapTo_is_called)
					.Should(Map_the_property_values)
					.Verify();
			}

			private void Map_the_property_values(InputClass source)
			{
				var tester = new MappingTester<OutputClass>();
				var result = tester.Verify(_destination, _expected);
				result.IsValid.ShouldBeTrue();
				result.ToString().ShouldBeEqualTo("");
			}

			private void MapTo_is_called(InputClass source)
			{
				_destination = source.MapTo<OutputClass>();
				_expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty,
						DateTimeToNullable = source.DateTimeToNullable
					};
			}
		}
	}
}