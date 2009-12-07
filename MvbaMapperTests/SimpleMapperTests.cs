using MvbaMapper;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class SimpleMapperTests
	{
		[TestFixture]
		public class When_asked_to_map
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
			public void Should_populate_properties_with_same_name_and_assignable_type()
			{
				var expected = new OutputClass
					{
						BooleanProperty = _source.BooleanProperty,
						IntegerProperty = _source.IntegerProperty,
						StringProperty = _source.StringProperty
					};
				var actual = new OutputClass();
				new SimpleMapper<InputClass, OutputClass>().Map(_source, actual);
				tester.Verify(actual, expected);
			}
		}
	}
}