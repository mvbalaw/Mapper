using System;

using FluentAssert;

using MvbaCore;

using MvbaMapper;

using MvbaMapperTests.TestClasses;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class SimpleMapperTests
	{
		[TestFixture]
		public class When_asked_to_map
		{
			[Test]
			public void Given_a_null_destination()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new ObjectSourceContext())
					.When(Map_is_called)
					.With(A_null_destination)
					.ShouldThrowException<ArgumentNullException>()
					.Verify();
			}

			[Test]
			public void Given_a_null_source()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new LowerCaseContext())
					.When(Map_is_called)
					.With(A_null_source)
					.Should(Not_change_the_destination_object)
					.Verify();
			}

			[Test]
			public void Given_inputs_where_the_cases_of_the_property_names_differ()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new LowerCaseContext())
					.When(Map_is_called)
					.With(Inputs_whose_property_names_have_different_casing)
					.Should(Map_the_property_values)
					.Verify();
			}

			[Test]
			public void Given_source_passed_as_object_type()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new ObjectSourceContext())
					.When(Map_is_called)
					.With(Source_passed_as_object_type)
					.Should(Map_the_property_values)
					.Verify();
			}

			private static void A_null_destination(SimpleMapper arg1, ObjectSourceContext context)
			{
				context.Source = new ClassFiller<InputClass>().Source;
				context.Destination = null;
			}

			private static void A_null_source(SimpleMapper obj, LowerCaseContext context)
			{
				context.Source = null;
				context.Destination = new OutputClassLowerCase();
				context.Expected = new OutputClassLowerCase();
			}

			private static void Inputs_whose_property_names_have_different_casing(SimpleMapper obj, LowerCaseContext context)
			{
				context.Source = new ClassFiller<InputClass>().Source;
				context.Destination = new OutputClassLowerCase();
				context.Expected = new OutputClassLowerCase
					{
						strIngPropErty = context.Source.StringProperty,
					};

				// sanity check that the property names are different by case alone
				string inputPropertyName = Reflection.GetPropertyName((InputClass input) => input.StringProperty);
				string actualPropertyName = Reflection.GetPropertyName((OutputClassLowerCase x) => x.strIngPropErty);
				actualPropertyName.ShouldNotBeEqualTo(inputPropertyName);
				String.Compare(actualPropertyName, inputPropertyName, true).ShouldBeEqualTo(0);
			}

			private static void Map_is_called(SimpleMapper simpleMapper, ObjectSourceContext context)
			{
				simpleMapper.Map(context.Source, context.Destination);
			}

			private static void Map_is_called(SimpleMapper obj, LowerCaseContext context)
			{
				obj.Map(context.Source, context.Destination);
			}

			private static void Map_the_property_values(SimpleMapper obj, LowerCaseContext context)
			{
				var result = new MappingTester<OutputClassLowerCase>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Map_the_property_values(SimpleMapper obj, ObjectSourceContext context)
			{
				var result = new MappingTester<OutputClass>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Not_change_the_destination_object(SimpleMapper obj, LowerCaseContext context)
			{
				var result = new MappingTester<OutputClassLowerCase>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Source_passed_as_object_type(SimpleMapper obj, ObjectSourceContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Source = source;
				context.Expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty,
						DateTimeToNullable = source.DateTimeToNullable
					};
				context.Destination = new OutputClass();
			}
		}
	}
}