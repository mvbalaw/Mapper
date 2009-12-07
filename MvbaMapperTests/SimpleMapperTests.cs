using System;

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
				new SimpleMapper().Map(_source, actual);
				tester.Verify(actual, expected);
			}

			[Test]
			public void Should_populate_properties_from_source_when_passed_as_object()
			{
				var expected = new OutputClass
					{
						BooleanProperty = _source.BooleanProperty,
						IntegerProperty = _source.IntegerProperty,
						StringProperty = _source.StringProperty
					};
				var actual = new OutputClass();
				object src = _source;
				new SimpleMapper().Map(src, actual);
				tester.Verify(actual, expected);
			}

			[Test]
			public void Should_not_throw_exception_if_the_source_is_null()
			{
				var actual = new OutputClass();
				_source = null;
				new SimpleMapper().Map(_source, actual);
			}

			[Test, ExpectedException(typeof(ArgumentNullException))]
			public void Should_throw_exception_if_the_destination_is_null()
			{
				const OutputClass actual = null;
				new SimpleMapper().Map(_source, actual);
			}
		}
	}
}