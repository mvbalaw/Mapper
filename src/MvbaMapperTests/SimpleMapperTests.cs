using System;
using System.Linq;

using FluentAssert;

using MvbaCore;

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
			private MappingTester<OutputClass> tester;

			[SetUp]
			public void BeforeEachTest()
			{
				tester = new MappingTester<OutputClass>();
				_source = new ClassFiller<InputClass>().Source;
			}

			[Test]
			public void Should_ignore_Property_name_case()
			{
				var expected = new OutputClassLowerCase
					{
						strIngPropErty = _source.StringProperty,
					};

				Reflection.GetPropertyName((OutputClassLowerCase x) => x.strIngPropErty).First().ShouldBeEqualTo('s');

				var actual = new OutputClassLowerCase();
				new SimpleMapper().Map(_source, actual);
				var result = new MappingTester<OutputClassLowerCase>().Verify(actual, expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			[Test]
			public void Should_not_throw_exception_if_the_source_is_null()
			{
				var actual = new OutputClass();
				_source = null;
				new SimpleMapper().Map(_source, actual);
			}

			[Test]
			public void Should_populate_properties_from_source_when_passed_as_object()
			{
				var expected = new OutputClass
					{
						BooleanProperty = _source.BooleanProperty,
						IntegerProperty = _source.IntegerProperty,
						StringProperty = _source.StringProperty,
						DecimalProperty = _source.DecimalProperty,
						DateTimeProperty = _source.DateTimeProperty,
						DateTimeToNullable = _source.DateTimeToNullable
					};
				var actual = new OutputClass();
				object src = _source;
				new SimpleMapper().Map(src, actual);
				var result = tester.Verify(actual, expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			[Test]
			public void Should_populate_properties_with_same_name_and_assignable_type()
			{
				var expected = new OutputClass
					{
						BooleanProperty = _source.BooleanProperty,
						IntegerProperty = _source.IntegerProperty,
						StringProperty = _source.StringProperty,
						DecimalProperty = _source.DecimalProperty,
						DateTimeProperty = _source.DateTimeProperty,
						DateTimeToNullable = _source.DateTimeToNullable
					};
				var actual = new OutputClass();
				new SimpleMapper().Map(_source, actual);
				var result = tester.Verify(actual, expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			[Test]
			public void Should_throw_exception_if_the_destination_is_null()
			{
				const OutputClass actual = null;
				Assert.Throws(typeof(ArgumentNullException), () => new SimpleMapper().Map(_source, actual));
			}
		}
	}
}