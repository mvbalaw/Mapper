using MvbaMapper;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class TExtensionsTests
	{
		[TestFixture]
		public class When_asked_to_map_from_another_class
		{
			[Test]
			public void Should_populate_properties()
			{
				var tester = new MappingTester<InputClass, OutputClass>();
				var source = tester.Source;
				var expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty
					};
				OutputClass actual = new OutputClass();
				actual.MapFrom(source);
				tester.Verify(actual, expected);
			}
		}

		[TestFixture]
		public class When_asked_to_map_to_a_specified_type
		{
			[Test]
			public void Should_populate_properties()
			{
				var tester = new MappingTester<InputClass, OutputClass>();
				var source = tester.Source;
				var expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty
					};
				OutputClass actual = source.MapTo<OutputClass>();
				tester.Verify(actual, expected);
			}
		}
	}
}